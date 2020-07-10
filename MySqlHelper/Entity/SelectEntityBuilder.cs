using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.Interfaces;
using MySqlHelper.QueryBuilder;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;

namespace MySqlHelper.Entity
{
    public class SelectEntityBuilder<T> : ISelectQuery<SelectEntityBuilder<T>> where T: new()
    {
        private readonly string connectionString;
        private readonly SelectQueryBuilder selectQueryBuilder;
        private readonly List<(Type tableObj, string leftTable, string rightTable)> withSubItems = new List<(Type tableObj, string leftTable, string rightTable)>();

        public SelectEntityBuilder(string connectionString)
        {
            this.connectionString = connectionString;
            this.selectQueryBuilder = new SelectQueryBuilder();
        }

        public SelectEntityBuilder<T> WithJoin(JoinEnum joinType, string leftTable, string rightTable,
            (string leftColumn, string rightColumn) columnConnection,
            params (string leftColumn, string rightColumn)[] columnsConnection)
        {
            selectQueryBuilder.WithJoin(joinType, leftTable, rightTable, columnConnection, columnsConnection);
            return this;
        }

        public SelectEntityBuilder<T> WithSubItems(string leftTable, string rightTable)
        {
            withSubItems.Add((typeof(T), leftTable, rightTable));
            return this;
        }

        public SelectEntityBuilder<T> WithWhere(WhereQueryCondition condition,
            params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            selectQueryBuilder.WithWhere(condition, wheres);
            return this;
        }

        public SelectEntityBuilder<T> WithWhere<T2>(WhereQueryCondition condition,
            params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres) where T2 : new()
        {
            selectQueryBuilder.WithWhere<T2>(condition, wheres);
            return this;
        }

        public SelectEntityBuilder<T> WithWhereAppend(WhereQuerySyntaxEnum syntax, WhereQueryCondition condition,
            params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            selectQueryBuilder.WithWhereAppend(syntax, condition, wheres);
            return this;
        }

        public SelectEntityBuilder<T> WithColumns(string column, params string[] columns)
        {
            selectQueryBuilder.WithColumns(column, columns);
            return this;
        }

        public SelectEntityBuilder<T> WithColumns<T2>(string column, params string[] columns) where T2 : new()
        {
            selectQueryBuilder.WithColumns<T2>(column, columns);
            return this;
        }

        public SelectEntityBuilder<T> WithGroupBy(string column, params string[] columns)
        {
            selectQueryBuilder.WithGroupBy(column, columns);
            return this;
        }

        public SelectEntityBuilder<T> WithOrderBy((string column, OrderBySorted sorted) orderBy,
            params (string column, OrderBySorted sorted)[] orderByItems)
        {
            selectQueryBuilder.WithOrderBy(orderBy, orderByItems);
            return this;
        }

        public IList<T> Execute()
        {
            var query = selectQueryBuilder.Build<T>();

            var output = new List<T>();

            using (var exe = new DataBaseExecuteReader(connectionString, query))
            {
                while (exe.DataReader.Read())
                {
                    var item = new T();
                    ReadValues(exe, item, selectQueryBuilder.HasJoin());
                    output.Add(item);
                }
            }

            return output;
        }

        public void ReadValues<T2>(DataBaseExecuteReader exe, T2 item, bool hasJoin) where T2 : new()
        {
            var properties = typeof(T2).GetProperties().ToList();
            var columnsPropertiesFromQuery = selectQueryBuilder.GetQueryPropertiesAndColumns<T>().ToList();
            properties.ForEach(property =>
            {
                if (Attribute.IsDefined(property, typeof(ForeignKeyModelAttribute)))
                {
                    // Read the foreign key item only if there is joins
                    if (!hasJoin || !HasTablesJoin<T2>(property))
                        return;

                    var foreignKeyItem = Activator.CreateInstance(property.PropertyType);
                    var t = property.PropertyType;
                    var methodGetModelColumns = this.GetType().GetMethod("ReadValues");
                    var genericMethod = methodGetModelColumns.MakeGenericMethod(t);
                    object[] args =
                    {
                        exe, foreignKeyItem, hasJoin
                    };
                    genericMethod.Invoke(this, args);
                    property.SetValue(item, foreignKeyItem);
                }
                else
                {
                    var columnName = hasJoin
                        ? ColumnAttribute.GetColumnRenameQuery<T2>(property.Name)
                        : ColumnAttribute.GetColumnNameNoQuotes<T2>(property.Name);

                    // process column only if it was in the query
                    if (selectQueryBuilder.HasDefinedColumns() && !columnsPropertiesFromQuery.Exists(x => x.columnQuery.Equals(columnName)))
                        return;

                    var value = exe.DataReader[columnName];
                    property.SetValue(item, value, null);
                }
            });
        }

        private bool HasTablesJoin<T2>(PropertyInfo property) where T2 : new()
        {
            var t = property.PropertyType;
            var methodGetModelColumns = selectQueryBuilder.GetType().GetMethod("ContainsJoinWithTables");
            var genericMethod = methodGetModelColumns.MakeGenericMethod(typeof(T2), t);
            return (bool)genericMethod.Invoke(selectQueryBuilder, null);
        }
    }
}
