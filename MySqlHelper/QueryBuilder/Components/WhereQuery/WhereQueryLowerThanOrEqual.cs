namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryLowerThanOrEqual : WhereQueryInequality
    {
        public WhereQueryLowerThanOrEqual(string column, object value) : base(column, "<=", value)
        {
        }
    }
}