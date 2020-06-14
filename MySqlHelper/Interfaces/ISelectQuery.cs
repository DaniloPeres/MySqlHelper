using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;

namespace MySqlHelper.Interfaces
{
    public interface ISelectQuery<out T> : IJoinQuery<T>, IWhereQuery<T>
    {
        /// <summary>
        /// Append a list of columns of select return.
        /// </summary>
        /// <param name="columns">
        /// Columns as string
        /// It can be strings (eg: "COUNT(*)", "IF(500 < 1000, "YES", "NO")", "id")
        ///   or the name of context property (eg: nameof(Book.Id), nameof(Book.Title)
        /// </param>
        /// <returns>The T</returns>
        T WithColumns(string column, params string[] columns);

        T WithColumns<T2>(string column, params string[] columns) where T2 : new();
        T WithGroupBy(string column, params string[] columns);

        T WithOrderBy((string column, OrderBySorted sorted) orderBy,
            params (string column, OrderBySorted sorted)[] orderByItems);
    }
}
