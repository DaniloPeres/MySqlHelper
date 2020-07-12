using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
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
