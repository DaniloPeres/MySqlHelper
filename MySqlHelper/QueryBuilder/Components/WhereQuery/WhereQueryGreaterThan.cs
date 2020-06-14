namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public class WhereQueryGreaterThan : WhereQueryInequality
    {
        public WhereQueryGreaterThan(string column, object value) : base(column, ">", value)
        {
        }
    }
}
