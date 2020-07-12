using System;
using System.Collections.Generic;
using System.Linq;
using MySqlHelper.Attributes;

namespace MySqlHelper.QueryBuilder.Components.Joins
{
    [Serializable()]
    public class JoinQueryBuilder
    {
        private readonly List<JoinQueryItem> joins = new List<JoinQueryItem>();

        internal void WithJoin(
            JoinEnum joinType,
            string leftTable,
            string rightTable,
            (string leftColumn, string rightColumn) columnConnection,
            params (string leftColumn, string rightColumn)[] columnsConnection)
        {
            joins.Add(new JoinQueryItem(joinType, leftTable, rightTable, columnConnection, columnsConnection));
        }

        internal string Build()
        {
            if (IsEmpty())
                return string.Empty;

            var joinStrings = joins.Select(join => join.GenerateJoinQuery());
            return $" {string.Join(" ", joinStrings)}";
        }

        internal bool IsEmpty()
        {
            return !joins.Any();
        }

        internal bool ContainsJoinWithTables<T1, T2>() where T1 : new() where T2 : new()
        {
            var tableNameLeft = TableAttribute.GetTableNameWithQuotes<T1>();
            var tableNameRight = TableAttribute.GetTableNameWithQuotes<T2>();

            return ContainsJoinWithTables(tableNameLeft, tableNameRight);
        }

        internal bool ContainsJoinWithTables(string leftTable, string rightTable)
        {
            return joins.Exists(x => x.LeftTable.Equals(leftTable) && x.RightTable.Equals(rightTable));
        }
    }
}
