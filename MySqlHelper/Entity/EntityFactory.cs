using CSharpFunctionalExtensions;
using MySql.Data.MySqlClient;
using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MySqlHelper.Entity
{
    public class EntityFactory
    {
        private readonly string connectionString;

        public EntityFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SelectEntityBuilder<T> CreateSelectBuilder<T>() where T : new()
        {
            return new SelectEntityBuilder<T>(connectionString);
        }

        public void Delete<T>(T model) where T : new()
        {
            DeleteEntity.Delete(connectionString, model);
        }

        public void Insert<T>(T entity, bool getLastId = true) where T : new()
        {
            InsertEntity.Insert(connectionString, entity, getLastId);
        }

        public void InsertMultiples<T>(IList<T> entities, bool getLastId = true) where T : new()
        {
            InsertEntity.InsertMultiples(connectionString, entities, getLastId);
        }

        public void Replace<T>(T entity) where T : new()
        {
            ReplaceEntity.Replace(connectionString, entity);
        }

        public void Update<T>(T model) where T : new()
        {
            UpdateEntity.Update(connectionString, model);
        }


        public void Update<T>(T model, params string[] fields) where T : new()
        {
            UpdateEntity.Update(connectionString, model, fields);
        }

        public void ExecuteNonQuery(string query)
        {
            DataBaseDataReader.ExecuteNonQuery(connectionString, query);
        }

        public void ExecuteNonQuery(string query, List<MySqlParameter> parameters)
        {
            DataBaseDataReader.ExecuteNonQuery(connectionString, query, parameters);
        }

        public Result<T> GetSelectValue<T>(string query, List<MySqlParameter> parameters = null)
        {
            return DataBaseDataReader.GetSelectValue<T>(connectionString, query, parameters);
        }

        public ulong GetLastInsertId()
        {
            return DataBaseDataReader.GetLastInsertId(connectionString);
        }

        internal static Dictionary<string, object> GetFieldsWithValues<T>(T entity, bool ignoreAutoIncrementKey) where T : new()
        {
            return GetFieldsWithValues(entity, typeof(T).GetProperties().ToList(), ignoreAutoIncrementKey);
        }

        internal static Dictionary<string, object> GetFieldsWithValues<T>(T entity, IList<string> fields, bool ignoreAutoIncrementKey) where T : new()
        {
            var properties = typeof(T).GetProperties().Where(x => fields.Contains(x.Name));
            return GetFieldsWithValues(entity, properties.ToList(), ignoreAutoIncrementKey);
        }

        internal static Dictionary<string, object> GetFieldsWithValues<T>(T entity, List<PropertyInfo> properties, bool ignoreAutoIncrementKey) where T : new()
        {
            var output = new Dictionary<string, object>();
            properties.ForEach(property =>
            {
                if (Attribute.IsDefined(property, typeof(ForeignKeyModelAttribute))
                    || Attribute.IsDefined(property, typeof(IgnoreAttribute))
                    || (ignoreAutoIncrementKey && KeyAttribute.IsAutoIncrementKey(property)))  // Ignore Auto increment key
                    return;

                var columnName = ColumnAttribute.GetColumnName(typeof(T), property.Name);
                output.Add(columnName, property.GetValue(entity));
            });

            return output;
        }
    }
}
