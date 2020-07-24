using System;
using MySqlHelper.Attributes;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public abstract class WhereQueryCondition
    {
        internal string Table;
        internal readonly string Column;
        internal readonly object Value;

        internal WhereQueryCondition(string column, object value)
        {
            Column = column;
            Value = value;
        }

        internal void SetTable<T>() where T : new()
        {
            Table = TableAttribute.GetTableNameWithQuotes<T>();
        }

        internal abstract string GenerateCondition();
        //internal abstract (string condition, MySqlParameter mySqlParameter) GenerateConditionWithMySqlParameter(string table);

        internal string PrependTable()
        {
            return string.IsNullOrEmpty(Table)
                ? string.Empty
                : $"{Table}.";
        }
    }
}
