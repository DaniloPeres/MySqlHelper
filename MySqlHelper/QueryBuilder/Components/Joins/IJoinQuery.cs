namespace MySqlHelper.QueryBuilder.Components.Joins
{
    public interface IJoinQuery<out T>
    {
        T WithJoin(
            JoinEnum joinType,
            string leftTable,
            string rightTable,
            (string leftColumn, string rightColumn) columnConnection,
            params (string leftColumn, string rightColumn)[] columnsConnection);
    }
}
