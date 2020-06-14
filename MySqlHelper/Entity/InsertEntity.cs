using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.QueryBuilder;

namespace MySqlHelper.Entity
{
    public static class InsertEntity
    {
        public static void Insert<T>(string connectionString, T entity) where T : new()
        {
            Insert<T>(connectionString, new List<T> { entity });
        }

        public static void Insert<T>(string connectionString, IList<T> entities) where T : new()
        {
            entities.ToList().ForEach(entity =>
            {
                var insertBuilder = new InsertQueryBuilder()
                    .WithFields(GetFieldsWithValues(entity));

                var query = insertBuilder.Build<T>();
                DataBaseDataReader.ExecuteNonQuery(connectionString, query);
            });
        }

        private static Dictionary<string, object> GetFieldsWithValues<T>(T entity) where T : new()
        {
            var output = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties().ToList();
            properties.ForEach(property =>
            {
                if (Attribute.IsDefined(property, typeof(ForeignKeyModelAttribute)))
                    return;

                var columnName = ColumnAttribute.GetColumnNameNoQuotes<T>(property.Name);
                output.Add(columnName, property.GetValue(entity));
            });

            return output;
        }
    }
}
