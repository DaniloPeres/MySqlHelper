using System;
using System.Reflection;

namespace MySqlHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; }
        public ColumnTypeEnum? Type { get; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }

        public ColumnAttribute(string name, ColumnTypeEnum type)
        {
            Name = name;
            Type = type;
        }

        public ColumnAttribute(ColumnTypeEnum type)
        {
            Type = type;
        }

        public static string GetColumnNameWithQuotes<T>(string property) where T : new()
        {
            return GetColumnNameWithQuotes(typeof(T), property);
        }

        public static string GetColumnNameWithQuotes(Type type, string property)
        {
            return $"`{GetColumnName(type, property)}`";
        }

        public static string GetColumnName(Type type, string property)
        {
            var info = type.GetProperty(property);

            if (info != null && Attribute.IsDefined(info, typeof(ColumnAttribute)))
            {
                var column = info.GetCustomAttribute<ColumnAttribute>();
                if (!string.IsNullOrEmpty(column.Name))
                    return column.Name;
            }

            return property;
        }

        public static string GetColumnNameWithTable(Type type, string property)
        {
            var tableName = TableAttribute.GetTableNameWithoutQuotes(type);

            var column = GetColumnName(type, property);
            return $"`{tableName}`.`{column}` `{GetColumnRenameQuery(type, property)}`";
        }

        public static string GetColumnRenameQuery(Type type, string property)
        {
            var tableName = TableAttribute.GetTableNameWithoutQuotes(type);
            var column = GetColumnName(type, property);
            return $"{tableName}_{column}";
        }
    }
}
