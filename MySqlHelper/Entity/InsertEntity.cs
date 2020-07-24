using System;
using System.Collections.Generic;
using System.Linq;
using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.QueryBuilder;

namespace MySqlHelper.Entity
{
    public static class InsertEntity
    {
        public static void Insert<T>(string connectionString, T entity) where T : new()
        {
            InsertMultiples(connectionString, new List<T> { entity });
        }

        public static void InsertMultiples<T>(string connectionString, IList<T> entities) where T : new()
        {
            entities.ToList().ForEach(entity =>
            {
                var insertBuilder = new InsertQueryBuilder()
                    .WithFields(EntityFactory.GetFieldsWithValues(entity, true));

                var query = insertBuilder.Build<T>();
                DataBaseDataReader.ExecuteNonQuery(connectionString, query);

                if (KeyAttribute.HasEntityAutoIncremenetalKey<T>())
                {
                    var keyProperty = KeyAttribute.GetAutoIncrementKeyProperty<T>();
                    var lastId = DataBaseDataReader.GetLastInsertId(connectionString);
                    keyProperty.SetValue(entity, Convert.ChangeType(lastId, keyProperty.PropertyType));
                }
            });
        }
    }
}
