namespace Data
{
    public class DbSet
    {
        public string TableName { get; init; }

        protected Dictionary<Guid, DbRecord> cachedRecords = new Dictionary<Guid, DbRecord>();
        protected DbContext dbContext;

        internal IList<DbRecord> toCreate = new List<DbRecord>();
        internal IList<DbRecord> toUpdate = new List<DbRecord>();
        internal IList<DbRecord> toDelete = new List<DbRecord>();

        public DbSet(string tableName, DbContext dbContext)
        {
            TableName = tableName;
            this.dbContext = dbContext;
        }
    }

    public class DbSet<T> : DbSet where T : DbRecord, new()
    {
        public DbSet(string tableName, DbContext dbContext) : base(tableName, dbContext)
        {
        }

        public T Get(Guid id)
        {
            T result;
            if (cachedRecords.ContainsKey(id))
            {
                return (T)cachedRecords[id];
            }
            else
            {
                result = new T();

                //dbContext.ExecuteReader(new NpgsqlCommand("SELECT * FROM @tableName WHERE id = @id").Prepare());


                cachedRecords.Add(id, result);
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
    }
}
