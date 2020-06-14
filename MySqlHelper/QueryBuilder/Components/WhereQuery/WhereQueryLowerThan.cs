namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryLowerThan : WhereQueryInequality
    {
        public WhereQueryLowerThan(string column, object value) : base(column, "<", value)
        {
        }
    }
}