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
        public static void Insert<T>(string connectionString, T entity, bool getLastId = true) where T : new()
        {
            InsertMultiples(connectionString, new List<T> { entity }, getLastId);
        }

        public static void InsertMultiples<T>(string connectionString, IList<T> entities, bool getLastId = true) where T : new()
        {
            var query = new StringBuilder();
            entities.ToList().ForEach(entity =>
            {
                var insertBuilder = new InsertQueryBuilder()
                    .WithFields(EntityFactory.GetFieldsWithValues(entity, true));

                query.Append(insertBuilder.Build<T>());
                query.Append(";");
            });

            if (getLastId && KeyAttribute.HasEntityAutoIncremenetalKey<T>())
            {
                query.Append("SELECT LAST_INSERT_ID()");
                using var exe = new DataBaseExecuteReader(connectionString, query.ToString());
                if (exe.DataReader.Read())
                {
                    var keyProperty = KeyAttribute.GetAutoIncrementKeyProperty<T>();
                    var lastId = (int)exe.DataReader.GetULong(0) - entities.Count();
                    entities.ToList().ForEach(entity =>
                    {
                        keyProperty.SetValue(entity, Convert.ChangeType(++lastId, keyProperty.PropertyType));
                    });
                }
            }
            else
                DataBaseDataReader.ExecuteNonQuery(connectionString, query.ToString());
        }
    }
}
