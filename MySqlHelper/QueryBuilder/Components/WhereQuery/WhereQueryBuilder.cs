using System;
using System.Linq;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    internal class WhereQueryBuilder
    {
        private WhereQueries whereQueries;

        internal void WithWhere(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            if (whereQueries != null)
                throw new Exception("The method 'WithWhere' or 'WithWhereFromTable' should be used only once. For extra conditions please use the method 'WithWhereAppend'");

            whereQueries = new WhereQueries(condition, wheres.ToList());
        }

        internal void WithWhere<T>(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres) where T : new()
        {
            if (whereQueries != null)
                throw new Exception("The method 'WithWhere' or 'WithWhereFromTable' should be used only once. For extra conditions please use the method 'WithWhereAppend'");

            condition.SetTable<T>();
            var wheresByTable = wheres.ToList();
            wheresByTable.ForEach(where => where.condition.SetTable<T>());

            whereQueries = new WhereQueries(condition, wheresByTable);
        }

        internal void WithWhereAppend
            (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            if (whereQueries == null)
                throw new Exception("The method 'WithWhereAppend' must be used after using the method 'WithWhere'");

            whereQueries.AddConditionsGroup(syntax, new WhereQueries(condition, wheres.ToList()));
        }

        internal bool IsWhereEmpty()
        {
            return whereQueries == null;
        }

        internal string Build()
        {
            return whereQueries == null ? string.Empty : $" WHERE {whereQueries.GenerateWhereQuery()}";
        }
    }
}
