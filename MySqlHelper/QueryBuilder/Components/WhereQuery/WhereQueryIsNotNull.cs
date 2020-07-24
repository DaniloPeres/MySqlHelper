using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryIsNotNull : WhereQueryCondition
    {
        public WhereQueryIsNotNull(string column)
            : base(column, string.Empty)
        { }

        internal override string GenerateCondition()
        {
            return $"NOT {PrependTable()}{Column} IS NULL";
        }
    }
}
