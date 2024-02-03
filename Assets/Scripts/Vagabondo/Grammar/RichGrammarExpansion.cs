using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Vagabondo.Utils;

namespace Vagabondo.Grammar
{
    public partial class RichGrammar : SubstitutionGrammar
    {
        private static string refPattern = @"\#(.*?)\#";

        public string GenerateText(string startRule = null, HashSet<string> inputTags = null)
        {
            if (startRule == null)
                startRule = this.startRule;

            var variables = new Dictionary<string, string>();

            if (inputTags == null)
                inputTags = new HashSet<string>();
            return expandRef(startRule, variables, inputTags);
        }

        private string expandRef(string refStr, Dictionary<string, string> variables, HashSet<string> inputTags)
        {
            if (variables.ContainsKey(refStr))
                return expandExpression(variables[refStr], variables, inputTags);

            var rule = rules[refStr];
            var ruleVariables = rule.variables;
            foreach (var ruleVariableName in ruleVariables.Keys)
                variables[ruleVariableName] = ruleVariables[ruleVariableName];

            if (!honorsTags(rule, inputTags))
                return null;

            string resolvedValue = null;
            do
            {
                var chosenClause = RandomUtils.RandomChooseWeighted(rule.clauses);
                resolvedValue = expandExpression(chosenClause, variables, inputTags);
            } while (resolvedValue == null); //TODO: improve; avoid infinite loop if no rule honors tags at some point

            return resolvedValue;
        }


        private string expandRefWithModifiers(string ruleStr, Dictionary<string, string> variables, HashSet<string> inputTags)
        {
            var nestedRuleTokens = ruleStr.Split(".");
            var nestedRuleReference = nestedRuleTokens[0];
            var expandedRule = expandRef(nestedRuleReference, variables, inputTags);
            if (expandedRule == null)
                return null;
            for (int iModifier = 0; iModifier < nestedRuleTokens.Length - 1; iModifier++)
                expandedRule = RichGrammarModifiers.applyModifier(expandedRule, nestedRuleTokens[iModifier + 1]);

            return expandedRule;
        }

        private string expandExpression(string expression, Dictionary<string, string> variables, HashSet<string> inputTags)
        {
            var refMatches = Regex.Matches(expression, refPattern);
            if (refMatches.Count == 0)
                return expression;

            StringBuilder resolvedValue = new StringBuilder();

            int processedIndex = 0;
            foreach (Match regexMatch in refMatches)
            {
                resolvedValue.Append(expression.Substring(processedIndex, regexMatch.Index - processedIndex));
                var nestedRefStr = expression.Substring(regexMatch.Index + 1, regexMatch.Length - 2);
                var expandedRef = expandRefWithModifiers(nestedRefStr, variables, inputTags);
                if (expandedRef == null)
                    return null;

                resolvedValue.Append(expandedRef);
                processedIndex = regexMatch.Index + regexMatch.Length;
            }
            resolvedValue.Append(expression.Substring(processedIndex));

            return resolvedValue.ToString();
        }


        private static bool honorsTags(RichGrammarRule rule, HashSet<string> inputTags)
        {
            if (rule.tags.Count == 0)
                return true;

            if (inputTags.Count == 0)
                return true;

            foreach (var inputTag in inputTags)
            {
                if (rule.tags.Contains(inputTag))
                    return true;
            }

            return false;
        }
    }
}