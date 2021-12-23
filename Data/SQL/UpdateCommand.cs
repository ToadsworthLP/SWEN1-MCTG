using Npgsql;
using System.Text;

namespace Data.SQL
{
    internal class UpdateCommand<T> where T : DbRecord
    {
        private string table;
        private List<(string, object)> parameters;
        private T value;
        private List<string> filters;

        public UpdateCommand()
        {
            parameters = new List<(string, object)>();
            filters = new List<string>();
        }

        public UpdateCommand<T> Into(DbSet dbSet)
        {
            table = dbSet.TableName;
            return this;
        }

        public UpdateCommand<T> Set(T record)
        {
            value = record;
            return this;
        }

        public UpdateCommand<T> WhereEquals<TFilter>(string column, TFilter value)
        {
            string index = parameters.Count.ToString();

            filters.Add($"{column} = @{index}");
            parameters.Add((index, value));

            return this;
        }

        public UpdateCommand<T> AndWhereEquals<TFilter>(string column, TFilter value)
        {
            filters.Add($"AND");
            return WhereEquals(column, value);
        }

        public UpdateCommand<T> OrWhereEquals<TFilter>(string column, TFilter value)
        {
            filters.Add($"OR");
            return WhereEquals(column, value);
        }

        public int Run(DbContext context)
        {
            if (table == null) throw new InvalidOperationException("No table to execute the command on was set.");

            if (value == null) throw new InvalidOperationException("No object to update was set.");

            if (filters.Count == 0) throw new InvalidOperationException("No Where() condition was set.");

            string[] columnNames = DbRecordConverter.ObjectToColumnNames<T>().ToArray();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"UPDATE {table} SET ");
            for (int i = 0; i < columnNames.Count(); i++)
            {
                stringBuilder.Append($"{columnNames[i]} = @{parameters.Count + i}");
                if (i < columnNames.Count() - 1) stringBuilder.Append(" , ");
            }
            stringBuilder.Append($" WHERE {string.Join(' ', filters)}");

            int affected;
            using (NpgsqlCommand command = new NpgsqlCommand(stringBuilder.ToString(), context.DbConnection))
            {
                foreach (var entry in parameters)
                {
                    command.Parameters.AddWithValue(entry.Item1, entry.Item2);
                }

                DbRecordConverter.ObjectToCommandParameters(command, value, filters.Count);
                affected = command.ExecuteNonQuery();
            }

            return affected;
        }
    }
}
