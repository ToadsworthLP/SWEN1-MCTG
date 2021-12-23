using Npgsql;

namespace Data.SQL
{
    internal class SelectCommand<T> where T : DbRecord, new()
    {
        private NpgsqlCommand command;
        private string table;
        private List<string> filters;

        public SelectCommand()
        {
            command = new NpgsqlCommand();
            filters = new List<string>();
        }

        public SelectCommand<T> From(DbSet dbSet)
        {
            table = dbSet.TableName;
            return this;
        }

        public SelectCommand<T> WhereEquals<TFilter>(string column, TFilter value)
        {
            string index = command.Parameters.Count.ToString();

            filters.Add($"{column} = @{index}");
            command.Parameters.AddWithValue(index, value);

            return this;
        }

        public SelectCommand<T> AndWhereEquals<TFilter>(string column, TFilter value)
        {
            filters.Add($"AND");
            return WhereEquals(column, value);
        }

        public SelectCommand<T> OrWhereEquals<TFilter>(string column, TFilter value)
        {
            filters.Add($"OR");
            return WhereEquals(column, value);
        }

        public IEnumerable<T> Run(DbContext context)
        {
            if (table == null) throw new InvalidOperationException("No table to execute the command on was set.");

            if (filters.Count > 0)
            {
                command.CommandText = $"SELECT * FROM {table} WHERE {string.Join(' ', filters)}";
            }
            else
            {
                command.CommandText = $"SELECT * FROM {table}";
            }

            command.Connection = context.DbConnection;
            command.Prepare();
            NpgsqlDataReader reader = command.ExecuteReader();
            IEnumerable<T> results = DbRecordConverter.ReaderToObjects<T>(reader);

            return results;
        }
    }
}
