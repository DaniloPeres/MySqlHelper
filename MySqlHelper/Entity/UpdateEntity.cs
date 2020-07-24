using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace MySqlHelper.Entity
{
    public static class UpdateEntity
    {
        public static void Update<T>(string connectionString, T entity) where T : new()
        {
            Update(connectionString, entity, EntityFactory.GetFieldsWithValues(entity, true));
        }

        public static void Update<T>(string connectionString, T entity, params string[] fields) where T : new()
        {
            Update(connectionString, entity, EntityFactory.GetFieldsWithValues(entity, fields.ToList(), true));
        }

        private static void Update<T>(string connectionString, T entity, Dictionary<string, object> fieldsWithValues) where T : new()
        {
            var (condition, wheres) = KeyAttribute.GetKeysQueryWhere(entity);

            var updateBuilder = new UpdateQueryBuilder()
                .WithFields(fieldsWithValues)
                .WithWhere(condition, wheres.ToArray());

            var query = updateBuilder.Build<T>();
            DataBaseDataReader.ExecuteNonQuery(connectionString, query);
        }
    }
}
