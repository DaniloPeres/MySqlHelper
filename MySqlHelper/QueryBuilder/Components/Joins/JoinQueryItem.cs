using System;
using System.Collections.Generic;
using System.Linq;

namespace MySqlHelper.QueryBuilder.Components.Joins
{
    [Serializable()]
    internal class JoinQueryItem
    {
        private readonly JoinEnum joinType;
        internal readonly string LeftTable;
        internal readonly string RightTable;
        private readonly List<(string leftColumn, string rightColumn)> columnsConnection;

        internal JoinQueryItem(JoinEnum joinType, string leftTable, string rightTable, (string leftColumn, string rightColumn) columnConnection, params (string leftColumn, string rightColumn)[] columnsConnection)
        {
            this.joinType = joinType;
            LeftTable = leftTable;
            RightTable = rightTable;
            this.columnsConnection = new List<(string leftColumn, string rightColumn)> { columnConnection };
            this.columnsConnection.AddRange(columnsConnection.ToList());
        }

        internal string GenerateJoinQuery()
        {
            var columnsConnectionString = string.Join(", ", columnsConnection.Select(x => $"{LeftTable}.{x.leftColumn} = {RightTable}.{x.rightColumn}"));
            return $"{GetJoinString(joinType)} {RightTable} ON {columnsConnectionString}";
        }

        private static string GetJoinString(JoinEnum joinType)
        {
            switch (joinType)
            {
                case JoinEnum.Join:
                    return "JOIN";
                case JoinEnum.LeftJoin:
                    return "LEFT JOIN";
                case JoinEnum.RightJoin:
                    return "RIGHT JOIN";
                case JoinEnum.FullJoin:
                    return "FULL JOIN";
                default:
                    throw new Exception($"Join Type '{joinType}' is not valid.");
            }
        }
    }
}

