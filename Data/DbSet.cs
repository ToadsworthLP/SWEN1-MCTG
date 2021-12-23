using Data.SQL;

namespace Data
{
    public class DbSet
    {
        public string TableName { get; init; }

        protected DbContext dbContext;

        public DbSet(string tableName, DbContext dbContext)
        {
            TableName = tableName;
            this.dbContext = dbContext;
        }

        internal virtual void Commit() { }

        internal virtual void Rollback() { }
    }

    public class DbSet<T> : DbSet where T : DbRecord, new()
    {
        private Dictionary<Guid, T> cachedRecords = new Dictionary<Guid, T>();
        private IList<T> toCreate = new List<T>();
        private IList<T> toUpdate = new List<T>();
        private IList<T> toDelete = new List<T>();

        public DbSet(string tableName, DbContext dbContext) : base(tableName, dbContext) { }

        public T? Get(Guid id)
        {
            T? result;
            if (cachedRecords.ContainsKey(id))
            {
                return cachedRecords[id];
            }
            else
            {
                result = new SelectCommand<T>().From(this).WhereEquals(nameof(result.Id), id).Run(dbContext).FirstOrDefault();
                if (result != null) cachedRecords.Add(id, result);
            }

            return result;
        }

        public T Create(T record)
        {
            T newRecord = record with { Id = Guid.NewGuid() };
            toCreate.Add(newRecord);
            cachedRecords[record.Id] = record;

            return newRecord;
        }

        public void Update(T record)
        {
            toUpdate.Add(record);
            cachedRecords[record.Id] = record;
        }

        public void Delete(T record)
        {
            toDelete.Add(record);
        }

        internal override void Commit()
        {
            foreach (T record in toCreate)
            {
                new InsertCommand<T>().Into(this).Values(record).Run(dbContext);
            }
        }

        internal override void Rollback()
        {

        }
    }
}
