using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MySql.Data.Types;
using MySqlHelper.Attributes;
using MySqlHelper.DataBase;
using MySqlHelper.Interfaces;
using MySqlHelper.QueryBuilder;
using MySqlHelper.QueryBuilder.Components.Joins;
using MySqlHelper.QueryBuilder.Components.OrderBy;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using MySqlHelper.Utils;

namespace MySqlHelper.Entity
{
    public class SelectEntityBuilder<T> : ISelectQuery<SelectEntityBuilder<T>> where T : new()
    {
        private readonly string connectionString;
        private readonly SelectQueryBuilder selectQueryBuilder;
        private readonly List<(Type type, Type typeSubItem)> withSubItems = new List<(Type tableObj, Type typeSubItem)>();

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

        public SelectEntityBuilder<T> WithSubItems(Type typeSubItem)
        {
            withSubItems.Add((typeof(T), typeSubItem));
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
            var items = ExecuteMainQuery();
            withSubItems.ForEach(subItem => ExecuteSubItemsQuery(items, subItem));

            return items;
        }

        public void ReadValues<T2>(DataBaseExecuteReader exe, Type type, T2 item, bool hasJoin, bool readingSubItems) where T2 : new()
        {
            var properties = type.GetProperties().ToList();
            var columnsPropertiesFromQuery = selectQueryBuilder.GetQueryPropertiesAndColumns(typeof(T2)).ToList();
            properties.ForEach(property =>
            {
                if (Attribute.IsDefined(property, typeof(IgnoreAttribute)))
                    return;

                if (Attribute.IsDefined(property, typeof(ForeignKeyModelAttribute)))
                {
                    // Read the foreign key item only if there is joins
                    if (property.PropertyType.IsList() || !hasJoin || !HasTablesJoin(type, property))
                        return;

                    var foreignKeyItem = Activator.CreateInstance(property.PropertyType);
                    var t = property.PropertyType;
                    var methodGetModelColumns = this.GetType().GetMethod("ReadValues");
                    var genericMethod = methodGetModelColumns.MakeGenericMethod(t);
                    object[] args =
                    {
                        exe, t, foreignKeyItem, hasJoin, readingSubItems
                    };
                    genericMethod.Invoke(this, args);
                    property.SetValue(item, foreignKeyItem);
                }
                else
                {
                    var columnName = hasJoin
                        ? ColumnAttribute.GetColumnRenameQuery(type, property.Name)
                        : ColumnAttribute.GetColumnName(type, property.Name);

                    // process column only if it was in the query
                    if (!readingSubItems && !selectQueryBuilder.HasDefinedColumn(columnName))
                        return;

                    var value = exe.DataReader[columnName];

                    if (property.PropertyType == typeof(bool))
                        value = String2Bool(value.ToString());
                    else if (value.GetType() == typeof(MySqlDateTime))
                    {
                        var valueMyDateTime = (MySqlDateTime)value;
                        value = valueMyDateTime.IsValidDateTime ? valueMyDateTime.Value : default;
                    } else if (value.GetType() == typeof(System.DBNull))
                        value = GetDefault(property.PropertyType);
                    
                    property.SetValue(item, value, null);
                }
            });
        }

        public object GetDefault(Type t)
        {
            return this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
        }

        public T GetDefaultGeneric<T>()
        {
            return default(T);
        }

        private IList<T> ExecuteMainQuery()
        {
            var query = selectQueryBuilder.Build<T>();

            var output = new List<T>();

            using (var exe = new DataBaseExecuteReader(connectionString, query))
            {
                while (exe.DataReader.Read())
                {
                    var item = new T();
                    ReadValues(exe, typeof(T), item, selectQueryBuilder.HasJoin(), false);
                    output.Add(item);
                }
            }

            return output;
        }

        private static bool String2Bool(string str)
        {
            if (str == "on"
             || str == "yes"
             || str == "true"
            )
                return true;

            if ((str == "off")
             || (str == "no")
             || (str == "false")
            )
                return false;

            return atoi(str) == 1;
        }

        private static int atoi(string value)
        {
            int _return;
            if (value.Contains('x'))
                int.TryParse(value.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _return);
            else
                int.TryParse(value, out _return);
            return _return;
        }

        private void ExecuteSubItemsQuery(IList<T> items, (Type type, Type typeSubItem) subItem)
        {
            var leftTableColumn = TableAttribute.GetIdColumnNameWithQuotesAndTable<T>();
            var rightTableColumn = TableAttribute.GetForeignColumnIdNameWithQuotes(subItem.typeSubItem, subItem.type);

            var leftTable = TableAttribute.GetTableNameWithQuotes(subItem.type);
            var rightTable = TableAttribute.GetTableNameWithQuotes(subItem.typeSubItem);

            var selectQueryBuilderSubItem = selectQueryBuilder
                .CloneAsSelectQueryBuilder()
                .ClearColumns()
                .WithColumns($"{rightTable}.*")
                .WithJoin(JoinEnum.LeftJoin, leftTable, rightTable, (leftTableColumn, rightTableColumn));

            var whereQueryIsNotNull = new WhereQueryIsNotNull($"{rightTable}.{rightTableColumn}");

            selectQueryBuilderSubItem = selectQueryBuilderSubItem.IsWhereEmpty()
                ? selectQueryBuilderSubItem.WithWhere(whereQueryIsNotNull)
                : selectQueryBuilderSubItem.WithWhereAppend(WhereQuerySyntaxEnum.And, whereQueryIsNotNull);

            var query = selectQueryBuilderSubItem.Build<T>();

            using (var exe = new DataBaseExecuteReader(connectionString, query))
            {
                while (exe.DataReader.Read())
                {
                    ReadSubValues(items, subItem, exe);
                }
            }
        }

        private void ReadSubValues(IList<T> items, (Type tableObj, Type typeSubItem) subItemInfo, DataBaseExecuteReader exe)
        {
            var subItem = Activator.CreateInstance(subItemInfo.typeSubItem);
            ReadValues(exe, subItemInfo.typeSubItem, subItem, false, true);

            // 1 - get the id from subItem
            var subItemIdProperty =
                subItemInfo.typeSubItem.GetProperty(TableAttribute.GetForeignPropertyName(subItemInfo.typeSubItem, subItemInfo.tableObj));
            var subItemForeignId = subItemIdProperty.GetValue(subItem);

            // 2 - Find the item by subitem id
            var item = GetItemById(items, subItemForeignId);

            // 3 - add the item in the list
            AddSubItemInTheList(item, subItem);
        }

        private void AddSubItemInTheList<T>(T item, object subItem)
        {
            var subItemType = subItem.GetType();

            var listSubItems = GetListOfSubItem(item, subItemType);
            listSubItems.Add(subItem);
        }

        private IList GetListOfSubItem<T2>(T2 item, Type subItemType)
        {
            var properties = typeof(T2).GetProperties().ToList();

            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType
                    && property.PropertyType.IsList()
                    && property.PropertyType.GetGenericArguments()[0] == subItemType
                    && item != null)
                {
                    var value = (IList)property.GetValue(item);
                    if (value == null)
                    {
                        var listGenericType = typeof(List<>);
                        var genericListType = listGenericType.MakeGenericType(subItemType);
                        value = (IList)Activator.CreateInstance(genericListType);
                        property.SetValue(item, value);
                    }

                    return value;
                }
                //if (Attribute.IsDefined(property, typeof(List<>)))
                //{
                //    return property.Name;
                //}
            }

            throw new Exception($"The class '{typeof(T2).Name}' has no property list of sub-item '{subItemType.Name}'");
        }
        private T GetItemById(IList<T> items, object id)
        {
            return items.ToList().Find(item =>
            {
                var itemIdProperty =
                    // 1 - get the id from subItem
                    item.GetType().GetProperty(TableAttribute.GetIdColumnPropertyName(item.GetType()));
                var itemId = itemIdProperty.GetValue(item);
                return itemId.Equals(id);
            });
        }

        private bool HasTablesJoin(Type type, PropertyInfo property)
        {
            var t = property.PropertyType;
            var methodGetModelColumns = selectQueryBuilder.GetType().GetMethod("ContainsJoinWithTables");
            var genericMethod = methodGetModelColumns.MakeGenericMethod(type, t);
            return (bool)genericMethod.Invoke(selectQueryBuilder, null);
        }
    }
}
