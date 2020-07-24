using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryIsNull : WhereQueryCondition
    {
        public WhereQueryIsNull(string column)
            : base(column, string.Empty)
        { }

        internal override string GenerateCondition()
        {
            return $"{PrependTable()}{Column} IS NULL";
        }
    }
}
