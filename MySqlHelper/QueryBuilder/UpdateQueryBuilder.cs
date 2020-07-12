using System.Collections.Generic;
using System.Linq;
using MySqlHelper.Attributes;
using MySqlHelper.QueryBuilder.Components.WhereQuery;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder
{
    public class UpdateQueryBuilder : IWhereQuery<UpdateQueryBuilder>
    {
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();
        private readonly WhereQueryBuilder whereBuilder = new WhereQueryBuilder();

        public UpdateQueryBuilder WithFields(Dictionary<string, object> fields)
        {
            foreach (var pair in fields)
            {
                this.fields.Add(pair.Key, pair.Value);
            }
            return this;
        }

        public UpdateQueryBuilder WithWhere(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres)
        {
            whereBuilder.WithWhere(condition, wheres);
            return this;
        }

        public UpdateQueryBuilder WithWhere<T>(WhereQueryCondition condition, params (WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)[] wheres) where T : new()
        {
            whereBuilder.WithWhere<T>(condition, wheres);
            return this;
        }

        public UpdateQueryBuilder WithWhereAppend
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
            var fieldsValues = fields.Select(pair => $"{pair.Key} = {ValueFormat.GenerateFormattedValueForQuery(pair.Value)}");
            return $"UPDATE {table} SET {string.Join(", ", fieldsValues)}{whereBuilder.Build()}";
        }
    }
}
