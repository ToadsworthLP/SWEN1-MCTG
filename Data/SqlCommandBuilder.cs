using Npgsql;
using System.Text;

namespace Data
{
    public abstract class SqlCommandBuilderStep
    {
        protected SqlCommandBuilderData data;

        protected SqlCommandBuilderStep(SqlCommandBuilderData data)
        {
            this.data = data;
        }
    }

    public class SqlCommandBuilderData
    {
        public IList<object> Parameters;
        public StringBuilder CommandBuilder;
        public NpgsqlCommand Command;
    }

    public class SqlCommandBuilder
    {
        private SqlCommandBuilderData data;

        public SqlCommandBuilder()
        {
            data = new SqlCommandBuilderData();
            data.CommandBuilder = new StringBuilder();
            data.Parameters = new List<object>();
        }

        public SelectCommandBuilder Select(params string[] values)
        {
            return new SelectCommandBuilder(data);
        }

        public class SelectCommandBuilder : SqlCommandBuilderStep
        {
            public SelectCommandBuilder(SqlCommandBuilderData data, params string[] values) : base(data)
            {
                if (values == null)
                {
                    data.CommandBuilder.Append("SELECT ");
                }
                else
                {
                    // TODO implement
                }
            }

            public SqlCommandBuilderFinalStep From(ref string table)
            {
                data.CommandBuilder.Append($"FROM @{data.Parameters.Count} ");
                var v = data.Command.Parameters.Add("test", NpgsqlTypes.NpgsqlDbType.Text);
                return new SqlCommandBuilderFinalStep(data);
            }
        }

        public class SqlCommandBuilderFinalStep : SqlCommandBuilderStep
        {
            public SqlCommandBuilderFinalStep(SqlCommandBuilderData data) : base(data)
            {
            }

            public string Build()
            {
                return data.CommandBuilder.Append(";").ToString();
            }
        }
    }
}
