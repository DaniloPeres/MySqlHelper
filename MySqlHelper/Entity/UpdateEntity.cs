using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.QueryBuilder;
using System.Linq;

namespace MySqlHelper.Entity
{
    public static class UpdateEntity
    {
        public static void Update<T>(string connectionString, T entity) where T : new()
        {
            var (condition, wheres) = KeyAttribute.GetKeysQueryWhere(entity);

            var updateBuilder = new UpdateQueryBuilder()
                .WithFields(EntityFactory.GetFieldsWithValues(entity))
                .WithWhere(condition, wheres.ToArray());

            var query = updateBuilder.Build<T>();
            DataBaseDataReader.ExecuteNonQuery(connectionString, query);
        }


    }
}
