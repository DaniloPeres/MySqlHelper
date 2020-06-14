namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    public interface IWhereQuery<out T>
    {
        /// <summary>
        /// Set a list of wheres to use in the query
        /// Used for the first part of where query.
        /// Eg: `FirstName` = 'James' and `LastName` = 'Bond'
        /// </summary>
        /// <param name="wheres">
        /// An array WhereQuery that is generate the where query.
        /// </param>
        /// <returns>The SelectQueryBuilder</returns>
        T WithWhere(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres);

        T WithWhere<T2>(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres) where T2 : new();
        
        T WithWhereAppend(WhereQuerySyntaxEnum syntax, WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres);
    }
}
