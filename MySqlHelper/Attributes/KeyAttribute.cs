using MySqlHelper.QueryBuilder.Components.WhereQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MySqlHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public bool AutoIncrement;

        public KeyAttribute() { }
        public KeyAttribute(bool autoIncrement)
        {
            AutoIncrement = autoIncrement;
        }

        public static IEnumerable<PropertyInfo> GetKeysProperties<T>()
        {
            var properties = typeof(T).GetProperties().ToList();
            return properties.Where(property => IsDefined(property, typeof(KeyAttribute)));
        }

        public static PropertyInfo GetAutoIncrementKeyProperty<T>()
        {
            var properties = typeof(T).GetProperties().ToList();
            return properties.FirstOrDefault(IsAutoIncrementKey);
        }

        public static bool HasEntityAutoIncremenetalKey<T>()
        {
            var properties = typeof(T).GetProperties().ToList();
            return properties.Exists(IsAutoIncrementKey);
        }

        public static bool IsAutoIncrementKey(PropertyInfo property)
        {
            return IsDefined(property, typeof(KeyAttribute)) && property.GetCustomAttribute<KeyAttribute>().AutoIncrement;
        }

        public static (WhereQueryCondition condition, IList<(WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)> wheres) GetKeysQueryWhere<T>(T entity)
        {
            var properties = typeof(T).GetProperties().ToList();
            var keys = properties
                .FindAll(property => IsDefined(property, typeof(KeyAttribute)))
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

            return (condition, wheres);
        }
    }
}
