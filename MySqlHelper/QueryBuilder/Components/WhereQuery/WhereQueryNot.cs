using System;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    public class WhereQueryNot<T> : WhereQueryCondition where T : WhereQueryCondition
    {
        private readonly T whereQuery;

        public WhereQueryNot(T whereQuery) : base(whereQuery.Column, whereQuery.Value)
        {
            this.whereQuery = whereQuery;
        }

        internal override string GenerateCondition()
        {
            var query = whereQuery.GenerateCondition();
            return $"NOT {query}";
        }


        
    }
}
