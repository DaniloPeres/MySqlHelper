using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryNotLike : WhereQueryNot<WhereQueryLike>
    {
        public WhereQueryNotLike(string column, string value) : base(new WhereQueryLike(column, value))
        {
        }
    }
}
