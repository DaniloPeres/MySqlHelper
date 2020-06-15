
using System;
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

        public static string GetTableName<T>() where T : new()
        {
            return $"`{GetTableNameWithoutQuotes<T>()}`";
        }

        public static string GetTableNameWithoutQuotes<T>() where T : new()
        {
            if (typeof(T).GetCustomAttribute(typeof(TableAttribute), true) is TableAttribute tableAttribute)
            {
                return tableAttribute.Name;
            }

            throw new Exception($"The class '{typeof(T).Name}' has no attribute 'Table'");
        }
    }
}
