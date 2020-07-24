using System;
using System.Linq;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryIn : WhereQueryCondition
    {
        public WhereQueryIn(string column, params object[] inValues)
            : base(column, ConvertValueWhereQueryCondition(inValues))
        {
        }

        private static string ConvertValueWhereQueryCondition(object[] inValues)
        {
            var inValuesFormatted = inValues.ToList().Select(ValueFormat.GenerateFormattedValueForQuery);
            return $"({string.Join(",", inValuesFormatted)})";
        }

        internal override string GenerateCondition()
        {
            return string.Format(GenerateConditionFormat(), ValueFormat.GenerateFormattedValueForQuery(Value));
        }

        private string GenerateConditionFormat()
        {
            return $"{PrependTable()}{Column} IN {Value}";
        }
    }
}
