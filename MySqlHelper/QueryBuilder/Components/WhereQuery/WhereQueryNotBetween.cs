using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryNotBetween : WhereQueryNot<WhereQueryBetween>
    {
        public WhereQueryNotBetween(string column, object betweenFrom, object betweenTo) : base(new WhereQueryBetween(column, betweenFrom, betweenTo))
        {
        }
    }
}
