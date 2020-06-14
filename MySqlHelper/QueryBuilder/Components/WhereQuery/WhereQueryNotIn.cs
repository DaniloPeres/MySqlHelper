namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryNotIn : WhereQueryNot<WhereQueryIn>
    {
        public WhereQueryNotIn(string column, params object[] inValues) : base(new WhereQueryIn(column, inValues))
        {
        }
    }
}
