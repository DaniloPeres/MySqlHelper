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
            var properties = typeof(T).GetProperties().ToList();
            var keys = properties
                .FindAll(property => Attribute.IsDefined(property, typeof(KeyAttribute)))
                .Select(property => (property.Name, property.GetValue(entity))).ToList();
            
            if (!keys.Any())
                throw new Exception($"The entity model '{typeof(T)}' must have at least 1 key.");

            WhereQueryCondition condition = null;
            var wheres = new List<(WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)>();

            var isFirst = true;
            keys.ForEach(key =>
            {
                var equalsCondition = new WhereQueryEquals(key.Name, key.Item2);
                if (isFirst)
                {
                    condition = equalsCondition;
                    isFirst = false;
                }
                else
                {
                    wheres.Add((WhereQuerySyntaxEnum.And, equalsCondition));
                }

            });

            var deleteQueryBuilder = new DeleteQueryBuilder()
                .WithWhere(condition, wheres.ToArray());

            var query = deleteQueryBuilder.Build<T>();

            DataBaseDataReader.ExecuteNonQuery(connectionString, query);
        }
    }
}
