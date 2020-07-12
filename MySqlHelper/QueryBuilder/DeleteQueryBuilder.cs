using System.Text;
using MySqlHelper.Attributes;
using MySqlHelper.Interfaces;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.WhereQuery;

namespace MySqlHelper.QueryBuilder
{
    public class DeleteQueryBuilder : IJoinQuery<DeleteQueryBuilder>, IWhereQuery<DeleteQueryBuilder>
    {
        private readonly JoinQueryBuilder jsonBuilder = new JoinQueryBuilder();
        private readonly WhereQueryBuilder whereBuilder = new WhereQueryBuilder();

        public DeleteQueryBuilder WithJoin(JoinEnum joinType, string leftTable, string rightTable,
            (string leftColumn, string rightColumn) columnConnection,
            params (string leftColumn, string rightColumn)[] columnsConnection)
        {
            jsonBuilder.WithJoin(joinType, leftTable, rightTable, columnConnection, columnsConnection);
            return this;
        }

        public DeleteQueryBuilder WithWhere(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            whereBuilder.WithWhere(condition, wheres);
            return this;
        }

        public DeleteQueryBuilder WithWhere<T>(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres) where T : new()
        {
            whereBuilder.WithWhere<T>(condition, wheres);
            return this;
        }

        public DeleteQueryBuilder WithWhereAppend
            (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            whereBuilder.WithWhereAppend(syntax, condition, wheres);
            return this;
        }

        public string Build<T>() where T : new()
        {
            var tableName = TableAttribute.GetTableNameWithQuotes<T>();
            return Build(tableName);
        }

        public string Build(string table)
        {
            var queryResult = new StringBuilder($"DELETE FROM {table}");
            queryResult.Append(jsonBuilder.Build());
            queryResult.Append(whereBuilder.Build());

            return queryResult.ToString();
        }
    }
}
