using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryLowerThanOrEqual : WhereQueryInequality
    {
        public WhereQueryLowerThanOrEqual(string column, object value) : base(column, "<=", value)
        {
        }
    }
}