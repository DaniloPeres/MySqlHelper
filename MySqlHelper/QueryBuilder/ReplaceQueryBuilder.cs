using System;
using System.Collections.Generic;
using System.Text;
using MySqlHelper.Attributes;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder
{
    public class ReplaceQueryBuilder
    {
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        public ReplaceQueryBuilder WithFields(Dictionary<string, object> fields)
        {
            foreach (var pair in fields)
            {
                this.fields.Add(pair.Key, pair.Value);
            }
            return this;
        }

        public string Build<T>() where T : new()
        {
            var tableName = TableAttribute.GetTableNameWithQuotes<T>();
            return Build(tableName);
        }

        public string Build(string table)
        {
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var pair in fields)
            {
                columns.Add(pair.Key);
                values.Add(ValueFormat.GenerateFormattedValueForQuery(pair.Value));
            }

            return $"REPLACE INTO {table} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
        }
    }
}
