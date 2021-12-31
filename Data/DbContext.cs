using Npgsql;

namespace Data
{
    public class DbContext : IDisposable
    {
        private IList<DbSet> dbSets = new List<DbSet>();
        public NpgsqlConnection DbConnection { get; init; }

        public DbContext(string connectionString)
        {
            DbConnection = new NpgsqlConnection(connectionString);
            DbConnection.Open();
        }

        public static void MapEnum<T>(string name) where T : struct, Enum
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<T>(name);
        }

        public void Bind<T>(string tableName, out DbSet<T> dbSet) where T : DbRecord, new()
        {
            dbSet = new DbSet<T>(tableName, this);
            dbSets.Add(dbSet);
        }

        public void Commit()
        {
            NpgsqlTransaction transaction = DbConnection.BeginTransaction();

            foreach (DbSet dbSet in dbSets)
            {
                dbSet.Commit();
            }

            transaction.Commit();
        }

        public void Rollback()
        {
            foreach (DbSet dbSet in dbSets)
            {
                dbSet.Rollback();
            }
        }

        public void Dispose()
        {
            DbConnection.Close();
            Console.WriteLine("closed");
        }
    }
}
