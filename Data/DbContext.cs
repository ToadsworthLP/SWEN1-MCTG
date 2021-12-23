using Npgsql;

namespace Data
{
    public class DbContext
    {
        private static object transactionLock = new object();

        private IList<DbSet> dbSets = new List<DbSet>();
        public NpgsqlConnection DbConnection { get; init; }

        public DbContext(string connectionString)
        {
            DbConnection = new NpgsqlConnection(connectionString);
            DbConnection.Open();
        }

        ~DbContext()
        {
            DbConnection.Close();
        }

        public void Bind<T>(string tableName, out DbSet<T> dbSet) where T : DbRecord, new()
        {
            dbSet = new DbSet<T>(tableName, this);
            dbSets.Add(dbSet);
        }

        public void Commit()
        {
            lock (transactionLock)
            {
                NpgsqlTransaction transaction = DbConnection.BeginTransaction();

                foreach (DbSet dbSet in dbSets)
                {
                    dbSet.Commit();
                }

                transaction.Commit();
            }
        }

        public void Rollback()
        {
            foreach (DbSet dbSet in dbSets)
            {
                dbSet.Rollback();
            }
        }
    }
}
