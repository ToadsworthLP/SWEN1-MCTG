using Npgsql;
using System.Text;

namespace Data.SQL
{
    internal class InsertCommand<T> where T : DbRecord
    {
        private NpgsqlCommand command;
        private string table;
        private T value;

        public InsertCommand()
        {
            command = new NpgsqlCommand();
        }

        public InsertCommand<T> Into(DbSet dbSet)
        {
            table = dbSet.TableName;
            return this;
        }

        public InsertCommand<T> Values(T record)
        {
            value = record;
            return this;
        }

        public int Run(DbContext context)
        {
            if (table == null) throw new InvalidOperationException("No table to execute the command on was set.");

            if (value == null) throw new InvalidOperationException("No object to insert was set.");

            IEnumerable<string> columnNames = DbRecordConverter.ObjectToColumnNames<T>();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT INTO {table} ({string.Join(',', columnNames)}) VALUES (");
            for (int i = 0; i < columnNames.Count(); i++)
            {
                stringBuilder.Append($"@{i}");
                if (i < columnNames.Count() - 1) stringBuilder.Append(',');
            }
            stringBuilder.Append(')');

            int affected;
            using (NpgsqlCommand command = new NpgsqlCommand(stringBuilder.ToString(), context.DbConnection))
            {
                DbRecordConverter.ObjectToCommandParameters(command, value);
                affected = command.ExecuteNonQuery();
            }

            return affected;
        }
    }
}
