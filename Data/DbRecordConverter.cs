using Npgsql;
using System.Reflection;

namespace Data
{
    internal class DbRecordConverter
    {
        public static IEnumerable<T> ReaderToObjects<T>(NpgsqlDataReader reader) where T : DbRecord, new()
        {
            List<T> results = new List<T>();

            // Cache FieldInfo to avoid unnecessary searches later on
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                properties.Add(property.Name.ToLower(), property);
            }

            while (reader.Read())
            {
                T result = new T();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo target;
                    if (properties.TryGetValue(reader.GetName(i).ToLower(), out target))
                    {
                        object value = reader.GetValue(i);
                        target.SetValue(result, value == DBNull.Value ? null : value);
                    }
                }

                results.Add(result);
            }

            return results;
        }

        public static IEnumerable<string> ObjectToColumnNames<T>()
        {
            List<string> columnNames = new List<string>();

            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                columnNames.Add(properties[i].Name.ToLower());
            }

            return columnNames;
        }

        public static void ObjectToCommandParameters<T>(NpgsqlCommand command, T record, int startIndex = 0) where T : DbRecord
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = startIndex; i < properties.Length + startIndex; i++)
            {
                object? value = properties[i - startIndex].GetValue(record);
                command.Parameters.AddWithValue(i.ToString(), value == null ? DBNull.Value : value);
            }
        }
    }
}
