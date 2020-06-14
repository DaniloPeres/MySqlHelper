namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryNotBetween : WhereQueryNot<WhereQueryBetween>
    {
        public WhereQueryNotBetween(string column, object betweenFrom, object betweenTo) : base(new WhereQueryBetween(column, betweenFrom, betweenTo))
        {
        }
    }
}
