using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryLowerThan : WhereQueryInequality
    {
        public WhereQueryLowerThan(string column, object value) : base(column, "<", value)
        {
        }
    }
}