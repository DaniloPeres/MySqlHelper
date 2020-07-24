using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryNotEquals : WhereQueryNot<WhereQueryEquals>
    {
        public WhereQueryNotEquals(string column, object value) : base(new WhereQueryEquals(column, value))
        {
        }
    }
}
