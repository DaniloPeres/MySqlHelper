using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySqlHelper.QueryBuilder.Components.WhereQuery
{
    [Serializable]
    internal class WhereQueries
    {
        private readonly WhereQueryCondition condition;
        private readonly List<(WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)> extraConditions;
        private readonly List<(WhereQuerySyntaxEnum syntax, WhereQueries whereQuery)> extraConditionsGroups = new List<(WhereQuerySyntaxEnum syntax, WhereQueries whereQueries)>();

        public WhereQueries(WhereQueryCondition condition, IList<(WhereQuerySyntaxEnum syntax, WhereQueryCondition condition)>  extraConditions)
        {
            this.condition = condition;
            this.extraConditions = extraConditions.ToList();
        }

        public void AddConditionsGroup(WhereQuerySyntaxEnum syntax, WhereQueries whereQueries)
        {
            extraConditionsGroups.Add((syntax, whereQueries));
        }

        public string GenerateWhereQuery()
        {
            var mainConditionString = condition.GenerateCondition();

            if (!extraConditions.Any() && !extraConditionsGroups.Any())
                return mainConditionString;

            var output = new StringBuilder(mainConditionString);

            if (extraConditions.Any())
            {
                var extraConditionsString = extraConditions.Select(x =>
                    $"{GenerateSyntaxString(x.syntax)}{x.condition.GenerateCondition()}");
                output.Append(string.Join(string.Empty, extraConditionsString));
            }

            if (extraConditionsGroups.Any())
            {
                output.Insert(0, "(");
                output.Append(")");
                
                extraConditionsGroups.ForEach(group =>
                {
                    output.Append(GenerateSyntaxString(group.syntax));
                    output.Append($"({group.whereQuery.GenerateWhereQuery()})");
                });
            }

            return output.ToString();
        }

        private string GenerateSyntaxString(WhereQuerySyntaxEnum syntax)
        {
            switch (syntax)
            {
                case WhereQuerySyntaxEnum.And:
                    return " AND ";
                case WhereQuerySyntaxEnum.Or:
                    return " OR ";
                default:
                    throw new Exception($"The Syntax '{syntax}' is not valid.");
            }
        }
    }
}
