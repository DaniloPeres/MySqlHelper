namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryGreaterThanOrEqual : WhereQueryInequality
    {
        public WhereQueryGreaterThanOrEqual(string column, object value) : base(column, ">=", value)
        {
        }
    }
}
