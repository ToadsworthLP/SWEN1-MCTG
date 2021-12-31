using Npgsql;

namespace Data
{
    public class DbContext
    {
        public const string ConnectionString = "Server=localhost;User Id=mctg;Password=pwd;Database=mctg;";

        // This avoids unnecessary database connections by always having at most one open per thread
        private static ThreadLocal<NpgsqlConnection?> ThreadLocalDbConnection = new ThreadLocal<NpgsqlConnection?>();

        public NpgsqlConnection DbConnection
        {
            get
            {
                if (ThreadLocalDbConnection.Value == null)
                {
                    ThreadLocalDbConnection.Value = new NpgsqlConnection(ConnectionString);
                    ThreadLocalDbConnection.Value.Open();
                }

                return ThreadLocalDbConnection.Value;
            }
        }

        private IList<DbSet> dbSets = new List<DbSet>();

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
    }
}
