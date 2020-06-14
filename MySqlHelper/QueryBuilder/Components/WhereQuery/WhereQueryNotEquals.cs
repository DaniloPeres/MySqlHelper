namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryNotEquals : WhereQueryNot<WhereQueryEquals>
    {
        public WhereQueryNotEquals(string column, object value) : base(new WhereQueryEquals(column, value))
        {
        }
    }
}
