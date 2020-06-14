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

        public static string GetColumnName<T>(string property) where T : new()
        {
            return $"`{GetColumnNameNoQuotes<T>(property)}`";
        }

        public static string GetColumnNameNoQuotes<T>(string property) where T : new()
        {
            var info = typeof(T).GetProperty(property);

            if (info != null && Attribute.IsDefined(info, typeof(ColumnAttribute)))
            {
                var column = info.GetCustomAttribute<ColumnAttribute>();
                if (!string.IsNullOrEmpty(column.Name))
                    return column.Name;
            }

            return property;
        }

        public static string GetColumnNameWithTable<T>(string property) where T : new()
        {
            var tableName = TableAttribute.GetTableNameWithoutQuotes<T>();

            var column = GetColumnNameNoQuotes<T>(property);
            return $"`{tableName}`.`{column}` `{GetColumnRenameQuery<T>(property)}`";
        }

        public static string GetColumnRenameQuery<T>(string property) where T : new()
        {
            var tableName = TableAttribute.GetTableNameWithoutQuotes<T>();
            var column = GetColumnNameNoQuotes<T>(property);
            return $"{tableName}_{column}";
        }
    }
}
