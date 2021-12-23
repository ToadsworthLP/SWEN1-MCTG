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

            return newRecord;
        }

        public void Update(T record)
        {
            toUpdate.Add(record);
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

            foreach (T record in toUpdate)
            {
                new UpdateCommand<T>().Into(this).Set(record).WhereEquals(nameof(record.Id), record.Id).Run(dbContext);
            }

            foreach (T record in toDelete)
            {
                new DeleteCommand().From(this).WhereEquals(nameof(record.Id), record.Id).Run(dbContext);
            }

            cachedRecords.Clear();
        }

        internal override void Rollback()
        {
            toCreate.Clear();
            toUpdate.Clear();
            toDelete.Clear();
        }
    }
}
