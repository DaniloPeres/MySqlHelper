using System;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryBetween : WhereQueryCondition
    {
        public WhereQueryBetween(string column, object betweenFrom, object betweenTo)
            : base(column, ConvertValueWhereQueryCondition(betweenFrom, betweenTo))
        { }

        private static string ConvertValueWhereQueryCondition(object betweenFrom, object betweenTo)
        {
            return $"{ValueFormat.GenerateFormattedValueForQuery(betweenFrom)} AND {ValueFormat.GenerateFormattedValueForQuery(betweenTo)}";
        }

        internal override string GenerateCondition()
        {
            return string.Format(GenerateConditionFormat(), ValueFormat.GenerateFormattedValueForQuery(Value));
        }

        private string GenerateConditionFormat()
        {
            return $"{PrependTable()}{Column} BETWEEN {Value}";
        }
    }
}
