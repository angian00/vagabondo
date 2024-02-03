using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Vagabondo.Grammar
{
    public interface IGrammarNoun
    {
        public string name { get; set; }
        public bool isPluralizable { get; set; }
    }

    public class RefExpansion
    {
        private static string refPattern = @"\#(.*?)\#";

        public static string ExpandText(string expression, string placeholder, List<IGrammarNoun> values)
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
                var expandedRef = expandRef(nestedRefStr, placeholder, values);
                if (expandedRef == null)
                    throw new System.Exception($"Cannot resolve ref: {nestedRefStr}");

                resolvedValue.Append(expandedRef);
                processedIndex = regexMatch.Index + regexMatch.Length;
            }
            resolvedValue.Append(expression.Substring(processedIndex));

            return resolvedValue.ToString();
        }

        private static string expandRef(string refStr, string placeholder, List<IGrammarNoun> values)
        {
            var refTokens = refStr.Split(".");
            var valueRef = refTokens[0];
            var matchingValue = expandValue(valueRef, placeholder, values);
            if (matchingValue == null)
                return null;

            //for (int iModifier = 0; iModifier < refTokens.Length - 1; iModifier++)
            //    matchingValue = RichGrammarModifiers.applyModifier(matchingValue, refTokens[iModifier + 1]);

            if (refTokens.Length == 1 || !matchingValue.isPluralizable)
                return matchingValue.name;

            if (refTokens[1] == "s")
                return RichGrammarModifiers.applyModifier(matchingValue.name, "s");

            throw new ArgumentException($"Unsupported reference modifier: {refTokens[1]}");
        }

        private static IGrammarNoun expandValue(string refStr, string placeholder, List<IGrammarNoun> values)
        {
            if (!refStr.StartsWith(placeholder))
                return null;

            var valueId = int.Parse(refStr.Substring(placeholder.Length)) - 1;
            if (valueId < 0 || valueId >= values.Count)
                return null;

            return values[valueId];
        }

    }
}