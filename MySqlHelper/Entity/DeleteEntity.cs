using System;
using System.Collections.Generic;
using System.Linq;
using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.QueryBuilder;
using MySqlHelper.QueryBuilder.Components.WhereQuery;

namespace MySqlHelper.Entity
{
    public static class DeleteEntity
    {
        public static void Delete<T>(string connectionString, T entity) where T : new()
        {
            var (condition, wheres) = KeyAttribute.GetKeysQueryWhere(entity);

            var deleteQueryBuilder = new DeleteQueryBuilder()
                .WithWhere(condition, wheres.ToArray());

            var query = deleteQueryBuilder.Build<T>();

            DataBaseDataReader.ExecuteNonQuery(connectionString, query);
        }
    }
}
