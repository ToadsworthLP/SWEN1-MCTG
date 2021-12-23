using Npgsql;

namespace Data.SQL
{
    internal class DeleteCommand
    {
        private string table;
        private List<(string, object)> parameters;
        private List<string> filters;

        public DeleteCommand()
        {
            parameters = new List<(string, object)>();
            filters = new List<string>();
        }

        public DeleteCommand From(DbSet dbSet)
        {
            table = dbSet.TableName;
            return this;
        }

        public DeleteCommand WhereEquals<TFilter>(string column, TFilter value)
        {
            string index = parameters.Count.ToString();

            filters.Add($"{column} = @{index}");
            parameters.Add((index, value));

            return this;
        }

        public DeleteCommand AndWhereEquals<TFilter>(string column, TFilter value)
        {
            filters.Add($"AND");
            return WhereEquals(column, value);
        }

        public DeleteCommand OrWhereEquals<TFilter>(string column, TFilter value)
        {
            filters.Add($"OR");
            return WhereEquals(column, value);
        }

        public int Run(DbContext context)
        {
            if (table == null) throw new InvalidOperationException("No table to execute the command on was set.");

            if (filters.Count == 0) throw new InvalidOperationException("No Where() condition was set.");

            string commandText = $"DELETE FROM {table} WHERE {string.Join(' ', filters)}";

            int affected;
            using (NpgsqlCommand command = new NpgsqlCommand(commandText, context.DbConnection))
            {
                foreach (var entry in parameters)
                {
                    command.Parameters.AddWithValue(entry.Item1, entry.Item2);
                }

                affected = command.ExecuteNonQuery();
            }

            return affected;
        }
    }
}
