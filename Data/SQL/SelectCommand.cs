using Npgsql;

namespace Data.SQL
{
    internal class SelectCommand
    {
        private NpgsqlCommand command;
        private string fromTable;
        private List<string> filters;

        public SelectCommand()
        {
            command = new NpgsqlCommand();
            filters = new List<string>();
        }

        public SelectCommand From(DbSet dbSet)
        {
            fromTable = dbSet.TableName;
            return this;
        }

        public SelectCommand WhereEquals<T>(string column, T value)
        {
            string index = command.Parameters.Count.ToString();

            filters.Add($"{column} = @{index}");
            command.Parameters.AddWithValue(index, value);

            return this;
        }

        public SelectCommand AndWhereEquals<T>(string column, T value)
        {
            filters.Add($"AND");
            return WhereEquals(column, value);
        }

        public SelectCommand OrWhereEquals<T>(string column, T value)
        {
            filters.Add($"OR");
            return WhereEquals(column, value);
        }

        public IEnumerable<T> Run<T>(DbContext context) where T : DbRecord, new()
        {
            if (fromTable == null) throw new InvalidOperationException("No table to execute the command on was set");

            if (filters.Count > 0)
            {
                command.CommandText = $"SELECT * FROM {fromTable} WHERE {string.Join(' ', filters)}";
            }
            else
            {
                command.CommandText = $"SELECT * FROM {fromTable}";
            }

            command.Connection = context.DbConnection;
            command.Prepare();

            NpgsqlDataReader reader = command.ExecuteReader();
            IEnumerable<T> results = DbRecordConverter.To<T>(reader);

            return results;
        }
    }
}
