using Npgsql;

namespace Data.SQL
{
    public class SelectCommand<T> where T : DbRecord, new()
    {
        private string table;
        private List<(string, object)> parameters;
        private List<string> filters;
        private string orderBy = "";

        public SelectCommand()
        {
            parameters = new List<(string, object)>();
            filters = new List<string>();
        }

        public SelectCommand<T> From(DbSet dbSet)
        {
            table = dbSet.TableName;
            return this;
        }

        public SelectCommand<T> WhereEquals<TFilter>(string column, TFilter value)
        {
            string index = parameters.Count.ToString();

            filters.Add($"{column} = @{index}");
            parameters.Add((index, value));

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

        public SelectCommand<T> OrderByAscending(string column)
        {
            orderBy = $" ORDER BY {column} ASC";
            return this;
        }

        public SelectCommand<T> OrderByDescending(string column)
        {
            orderBy = $" ORDER BY {column} DESC";
            return this;
        }

        public IEnumerable<T> Run(DbContext context)
        {
            if (table == null) throw new InvalidOperationException("No table to execute the command on was set.");

            string commandText;

            if (filters.Count > 0)
            {
                commandText = $"SELECT * FROM \"{table}\" WHERE {string.Join(' ', filters)} {orderBy}";
            }
            else
            {
                commandText = $"SELECT * FROM \"{table}\" {orderBy}";
            }

            IEnumerable<T> results;
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, context.DbConnection))
            {
                foreach (var entry in parameters)
                {
                    command.Parameters.AddWithValue(entry.Item1, entry.Item2);
                }

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    results = DbRecordConverter.ReaderToObjects<T>(reader);
                }
            }

            return results;
        }
    }
}
