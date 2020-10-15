using System;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryEquals : WhereQueryCondition
    {
        private bool caseSensitive;

        public WhereQueryEquals(string column, object value, bool caseSensitive = false) : base(column, value) {
            this.caseSensitive = caseSensitive;
        }
        
        internal override string GenerateCondition()
        {
            return string.Format(GenerateConditionFormat(), ValueFormat.GenerateFormattedValueForQuery(Value));
        }

        private string GenerateConditionFormat()
        {
            var binary = string.Empty;
            if (caseSensitive)
                binary = "BINARY ";
            return $"{binary}{PrependTable()}{Column} = {{0}}";
        }
    }
}
