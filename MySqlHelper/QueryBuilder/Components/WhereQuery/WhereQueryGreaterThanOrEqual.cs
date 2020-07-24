using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryGreaterThanOrEqual : WhereQueryInequality
    {
        public WhereQueryGreaterThanOrEqual(string column, object value) : base(column, ">=", value)
        {
        }
    }
}
