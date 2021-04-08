
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MySqlHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public virtual string Name { get; }

        public TableAttribute(string name)
        {
            Name = name;
        }

        public static string GetTableNameWithQuotes<T>() where T : new()
        {
            return GetTableNameWithQuotes(typeof(T));
        }

        public static string GetTableNameWithQuotes(Type type)
        {
            return $"`{GetTableNameWithoutQuotes(type)}`";
        }

        public static string GetIdColumnPropertyName<T>() where T : new()
        {
            return GetIdColumnPropertyName(typeof(T));
        }

        public static string GetIdColumnPropertyName(Type type)
        {
            var properties = type.GetProperties().ToList();

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(KeyAttribute)))
                {
                    return property.Name;
                }
            }

            throw new Exception($"The class '{type.Name}' has no property with an attribute 'Key'");
        }

        public static string GetIdColumnNameWithQuotesAndTable<T>() where T : new()
        {
            return GetIdColumnNameWithQuotesAndTable(typeof(T));
        }

        public static string GetIdColumnNameWithQuotesAndTable(Type type)
        {
            return ColumnAttribute.GetColumnNameWithQuotes(type, GetIdColumnPropertyName(type));
        }

        public static string GetTableNameWithoutQuotes<T>() where T : new()
        {
            return GetTableNameWithoutQuotes(typeof(T));
        }
        public static string GetTableNameWithoutQuotes(Type type)
        {
            if (type.GetCustomAttribute(typeof(TableAttribute), true) is TableAttribute tableAttribute)
            {
                return tableAttribute.Name;
            }

            throw new Exception($"The class '{type.Name}' has no attribute 'Table'");
        }

        public static string GetForeignColumnIdNameWithQuotes(Type type, Type typeForeign)
        {
            return $"`{GetForeignColumnIdName(type, typeForeign)}`";
        }

        public static string GetForeignColumnIdName(Type type, Type typeForeign)
        {
            return ColumnAttribute.GetColumnName(type, GetForeignPropertyName(type, typeForeign));
        }

        public static string GetForeignPropertyName(Type type, Type typeForeign)
        {
            return GetForeignProperty(type, typeForeign).Name;
        }

        private static PropertyInfo GetForeignProperty(Type type, Type typeForeign)
        {
            var properties = type.GetProperties().ToList();

            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(ForeignKeyIdAttribute), true) is ForeignKeyIdAttribute foreignKeyIdAttribute
                    && foreignKeyIdAttribute.ForeignType == typeForeign)
                {
                    return property;
                }
            }

            throw new Exception($"The class '{type.Name}' has no attribute 'ForeignKeyId' of '{typeForeign.Name}'");
        }
    }
}
