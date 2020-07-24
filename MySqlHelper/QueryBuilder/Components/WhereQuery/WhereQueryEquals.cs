using System;
using MySqlHelper.Utils;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryEquals : WhereQueryCondition
    {
        public WhereQueryEquals(string column, object value) : base(column, value) { }
        
        internal override string GenerateCondition()
        {
            return string.Format(GenerateConditionFormat(), ValueFormat.GenerateFormattedValueForQuery(Value));
        }

        //internal override (string condition, MySqlParameter mySqlParameter) GenerateConditionWithMySqlParameter(string table)
        //{
        //    var parameterName = $"@{table}_{column}";
        //    var condition = string.Format(GenerateConditionFormat(table), parameterName);
        //    var mySqlParameter = MySqlParameterUtils.CreateSqlParameter(parameterName, Value);

        //    return (condition, mySqlParameter);
        //}

        private string GenerateConditionFormat()
        {
            return $"{PrependTable()}{Column} = {{0}}";
        }
    }
}
