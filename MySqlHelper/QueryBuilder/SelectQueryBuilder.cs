using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using MySqlHelper.Attributes;
using MySqlHelper.Interfaces;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder
{
    [Serializable]
    public class SelectQueryBuilder : ISelectQuery<SelectQueryBuilder>, ICloneable
    {
        private readonly List<string> columns = new List<string>();
        private readonly JoinQueryBuilder joinBuilder = new JoinQueryBuilder();
        private readonly WhereQueryBuilder whereBuilder = new WhereQueryBuilder();
        private readonly List<string> groupBy = new List<string>();
        private readonly List<(string column, OrderBySorted sorted)> orderBy = new List<(string column, OrderBySorted sorted)>();

        public SelectQueryBuilder WithColumns(string column, params string[] columns)
        {
            this.columns.Add(column);
            this.columns.AddRange(columns.ToList());
            return this;
        }

        public SelectQueryBuilder ClearColumns()
        {
            columns.Clear();
            return this;
        }

        public SelectQueryBuilder WithColumns<T>(string column, params string[] columns) where T : new()
        {
            var tableName = TableAttribute.GetTableNameWithQuotes<T>();

            var newColumns = new List<string> { column };
            newColumns.AddRange(columns);

            var columnsWithTable = newColumns.Select(x => $"{tableName}.{x}");

            this.columns.AddRange(columnsWithTable);
            return this;
        }

        public SelectQueryBuilder WithJoin(
            JoinEnum joinType,
            string leftTable,
            string rightTable,
            (string leftColumn, string rightColumn) columnConnection,
            params (string leftColumn, string rightColumn)[] columnsConnection)
        {
            joinBuilder.WithJoin(joinType, leftTable, rightTable, columnConnection, columnsConnection);
            return this;
        }


        public SelectQueryBuilder WithWhere(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            whereBuilder.WithWhere(condition, wheres);
            return this;
        }

        public SelectQueryBuilder WithWhere<T>(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres) where T : new()
        {
            whereBuilder.WithWhere<T>(condition, wheres);
            return this;
        }

        public SelectQueryBuilder WithWhereAppend
            (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            whereBuilder.WithWhereAppend(syntax, condition, wheres);
            return this;
        }

        public bool IsWhereEmpty()
        {
            return whereBuilder.IsWhereEmpty();
        }

        public SelectQueryBuilder WithGroupBy(string column, params string[] columns)
        {
            groupBy.Add(column);
            groupBy.AddRange(columns);
            return this;
        }

        public SelectQueryBuilder WithOrderBy((string column, OrderBySorted sorted) orderBy,
            params (string column, OrderBySorted sorted)[] orderByItems)
        {
            this.orderBy.Add(orderBy);
            this.orderBy.AddRange(orderByItems);
            return this;
        }

        public string Build<T>() where T : new()
        {
            return Build(typeof(T));
        }

        public string Build(Type type)
        {
            var tableName = TableAttribute.GetTableNameWithQuotes(type);
            return Build(tableName, GenerateColumnsByModelQuery(type));
        }

        public string Build(string table)
        {
            return Build(table, GenerateColumnsQuery());
        }

        public bool ContainsJoinWithTables<T1, T2>() where T1 : new() where T2 : new()
        {
            return joinBuilder.ContainsJoinWithTables<T1, T2>();
        }

        private string Build(string table, string columns)
        {
            var queryResult = new StringBuilder("SELECT ");
            queryResult.Append(columns);
            queryResult.Append($" FROM {table}");
            queryResult.Append(joinBuilder.Build());
            queryResult.Append(whereBuilder.Build());
            queryResult.Append(GenerateGroupBy());
            queryResult.Append(GenerateOrderBy());

            return queryResult.ToString();
        }

        private string GenerateColumnsQuery()
        {
            return columns.Any()
                ? string.Join(", ", columns)
                : "*";
        }

        private string GenerateColumnsByModelQuery<T>() where T : new()
        {
            return GenerateColumnsByModelQuery(typeof(T));
        }

        private string GenerateColumnsByModelQuery(Type type)
        {
            return columns.Any()
                ? string.Join(", ", columns)
                : !HasJoin()
                    ? "*"
                    : string.Join(", ", GetModelColumns(type));
        }
        public bool HasJoin()
        {
            return !joinBuilder.IsEmpty();
        }

        public IEnumerable<string> GetModelColumns<T>() where T : new()
        {
            return GetModelColumns(typeof(T));
        }

        public IEnumerable<string> GetModelColumns(Type type)
        {
            return GetQueryPropertiesAndColumns(type).Select(x => x.columnQuery);
        }

        public IEnumerable<(PropertyInfo property, string columnQuery)> GetQueryPropertiesAndColumns(Type type)
        {
            var output = new List<(PropertyInfo property, string columnQuery)>();
            var joinColumns = new List<(PropertyInfo property, string columnQuery)>();
            var properties = type.GetProperties().ToList();

            properties.Sort((c1, c2) => string.Compare(c1.Name, c2.Name, StringComparison.Ordinal));

            properties.ForEach(property =>
            {
                if (Attribute.IsDefined(property, typeof(IgnoreAttribute)))
                    return;

                if (Attribute.IsDefined(property, typeof(ForeignKeyModelAttribute)))
                {
                    var t = property.PropertyType;
                    if (t.IsList())
                        return; // Do not process ForeignKeyModel list. It will get by sub-items
                    if (t.IsGenericType)
                        GetQueryPropertiesAndColumns(t = t.GetGenericArguments()[0]);
                    var methodGetModelColumns = this.GetType().GetMethod("GetQueryPropertiesAndColumns");
                    var foreignModelColumns = methodGetModelColumns.Invoke(this, new[] { t }) as List<(PropertyInfo property, string columnQuery)>;
                    joinColumns.AddRange(foreignModelColumns);
                }
                else
                {
                    output.Add((property, ColumnAttribute.GetColumnNameWithTable(type, property.Name)));
                }
            });

            output.AddRange(joinColumns);
            return output;
        }

        public bool HasDefinedColumn(string columnName)
        {
            return !columns.Any() || columns.Exists(x => x.Contains("*") || x == columnName || x.EndsWith($".{columnName}"));
        }

        public SelectQueryBuilder CloneAsSelectQueryBuilder()
        {
            return (SelectQueryBuilder)Clone();
        }

        public object Clone()
        {
            return DeepClone.Clone(this);
        }

        private string GenerateGroupBy()
        {
            return groupBy.Any() ? $" GROUP BY {string.Join(", ", groupBy)}" : string.Empty;
        }

        private string GenerateOrderBy()
        {
            if (!orderBy.Any())
                return string.Empty;

            var orderByString = orderBy.Select(x => $"{x.column} {GetOrderBySortedText(x.sorted)}");
            return $" ORDER BY {string.Join(", ", orderByString)}";
        }

        private string GetOrderBySortedText(OrderBySorted sorted)
        {
            switch (sorted)
            {
                case OrderBySorted.Asc:
                    return "ASC";
                case OrderBySorted.Desc:
                    return "DESC";
                default:
                    throw new Exception($"The sorted value '{sorted}' is not valid.");
            }
        }
    }
}
