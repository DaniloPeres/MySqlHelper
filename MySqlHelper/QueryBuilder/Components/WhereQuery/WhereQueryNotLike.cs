namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryNotLike : WhereQueryNot<WhereQueryLike>
    {
        public WhereQueryNotLike(string column, string value) : base(new WhereQueryLike(column, value))
        {
        }
    }
}
