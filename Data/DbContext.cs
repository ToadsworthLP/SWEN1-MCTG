﻿using Npgsql;

namespace Data
{
    public class DbContext
    {
        private IList<DbSet> dbSets = new List<DbSet>();
        public NpgsqlConnection DbConnection { get; init; }

        public DbContext(string connectionString)
        {
            DbConnection = new NpgsqlConnection(connectionString);
            DbConnection.Open();

            // TODO remove this testing stuff
            //NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM @table");
            //command.Parameters.AddWithValue("@table", "test");
            //command.Prepare();

            //NpgsqlDataReader reader = ExecuteReader(command);
            //while (reader.Read())
            //{
            //    for (int i = 0; i < reader.FieldCount; i++)
            //    {
            //        Console.WriteLine($"{reader.GetName(i)}: {reader.GetValue(i)}");
            //    }
            //}

            //DbConnection.Close();
        }

        public void Bind<T>(string tableName, out DbSet<T> dbSet) where T : DbRecord, new()
        {
            dbSet = new DbSet<T>(tableName, this);
            dbSets.Add(dbSet);
        }

        public void Commit()
        {
            foreach (DbSet dbSet in dbSets)
            {

            }
        }

        public void Rollback()
        {

        }
    }
}