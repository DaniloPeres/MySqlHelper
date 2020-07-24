using System;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryInequality : WhereQueryCondition
    {
        private readonly string inequalitySymbol;

        internal WhereQueryInequality(string column, string inequalitySymbol, object value) : base(column, value)
        {
            this.inequalitySymbol = inequalitySymbol;
        }

        internal override string GenerateCondition()
        {
            return $"{PrependTable()}{Column} {inequalitySymbol} {ValueFormat.GenerateFormattedValueForQuery(Value)}";
        }
    }
}
