using MySqlHelper.Attributes;
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

        public void Insert<T>(T entity) where T : new()
        {
            InsertEntity.Insert(connectionString, entity);
        }

        public void InsertMultiples<T>(IList<T> entities) where T : new()
        {
            InsertEntity.InsertMultiples(connectionString, entities);
        }

        public void Update<T>(T model) where T : new()
        {
            UpdateEntity.Update(connectionString, model);
        }

        internal static Dictionary<string, object> GetFieldsWithValues<T>(T entity) where T : new()
        {
            var output = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties().ToList();
            properties.ForEach(property =>
            {
                if (Attribute.IsDefined(property, typeof(ForeignKeyModelAttribute))
                || KeyAttribute.IsAutoIncrementKey(property))  // Ignore Auto increment key
                    return;

                var columnName = ColumnAttribute.GetColumnNameNoQuotes<T>(property.Name);
                output.Add(columnName, property.GetValue(entity));
            });

            return output;
        }
    }
}
