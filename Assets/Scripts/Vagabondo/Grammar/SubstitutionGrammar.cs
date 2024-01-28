using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Vagabondo.Utils;

namespace Vagabondo.Grammar
{
    public class SubstitutionGrammar
    {
        private static string ruleRefPattern = @"\#((?:\[.*\])?.*?)\#";
        private static string variablePattern = @"\[(.*?)\]";

        private Dictionary<string, List<string>> rules;

        private SubstitutionGrammar() { }

        public static SubstitutionGrammar Load(string filename)
        {

            var fileObj = Resources.Load<TextAsset>($"Data/Grammars/{filename}");

            var result = new SubstitutionGrammar();
            result.rules = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(fileObj.text);

            return result;
        }

        public string GenerateText(string startingRuleId = "origin")
        {
            var variables = new Dictionary<string, string>();

            return expandRule(startingRuleId, variables);
        }

        private string expandRule(string ruleId, Dictionary<string, string> variables)
        {
            if (variables.ContainsKey(ruleId))
                return variables[ruleId];

            var ruleValues = rules[ruleId];
            var rawValue = RandomUtils.RandomChoose(ruleValues);

            var ruleRefMatches = Regex.Matches(rawValue, ruleRefPattern);

            if (ruleRefMatches.Count == 0)
                return rawValue;

            StringBuilder resolvedValue = new StringBuilder();

            int processedIndex = 0;
            foreach (Match regexMatch in ruleRefMatches)
            {
                resolvedValue.Append(rawValue.Substring(processedIndex, regexMatch.Index - processedIndex));
                var nestedRuleStr = rawValue.Substring(regexMatch.Index + 1, regexMatch.Length - 2);

                var variableMatches = Regex.Matches(nestedRuleStr, variablePattern);
                foreach (Match variableMatch in variableMatches)
                {
                    var tokens = variableMatch.Groups[1].Value.Split(":");
                    string varName = tokens[0];
                    string varExpr = tokens[1];
                    if ((!varExpr.StartsWith("#")) || (!varExpr.EndsWith("#")))
                        throw new Exception($"This kind of variable expression is not supported: [{varExpr}]");

                    string varRuleReference = varExpr.Substring(1, varExpr.Length - 2);

                    string varValue = expandRuleWithModifiers(varRuleReference, variables);
                    variables.Add(varName, varValue);
                }

                string nestedRuleRef;
                if (variableMatches.Count == 0)
                    nestedRuleRef = nestedRuleStr;
                else
                    nestedRuleRef = nestedRuleStr.Substring(nestedRuleStr.LastIndexOf("]") + 1);
                var expandedNestedRule = expandRuleWithModifiers(nestedRuleRef, variables);

                resolvedValue.Append(expandedNestedRule);
                processedIndex = regexMatch.Index + regexMatch.Length;
            }
            resolvedValue.Append(rawValue.Substring(processedIndex));

            return resolvedValue.ToString();
        }

        private string expandRuleWithModifiers(string ruleStr, Dictionary<string, string> variables)
        {
            var nestedRuleTokens = ruleStr.Split(".");
            var nestedRuleReference = nestedRuleTokens[0];
            var expandedRule = expandRule(nestedRuleReference, variables);
            for (int iModifier = 0; iModifier < nestedRuleTokens.Length - 1; iModifier++)
                expandedRule = applyModifier(expandedRule, nestedRuleTokens[iModifier + 1]);

            return expandedRule.ToString();
        }


        private static string applyModifier(string originalText, string modifier)
        {
            if (modifier == "capitalize")
                return applyCapitalize(originalText);
            if (modifier == "capitalizeAll")
                return applyCapitalizeAll(originalText);
            if (modifier == "a")
                return applyA(originalText);
            if (modifier == "s")
                return applyPlural(originalText);
            if (modifier == "ed")
                return applyPastTense(originalText);

            return originalText;
        }

        private static string applyCapitalize(string originalText)
        {
            return originalText.Substring(0, 1).ToUpper() + originalText.Substring(1);
        }

        private static string applyCapitalizeAll(string originalText)
        {
            var tokens = originalText.Split();
            var result = new StringBuilder();
            foreach (var token in tokens)
                result.Append(applyCapitalize(token));

            return result.ToString();
        }

        private static string applyA(string originalText)
        {
            if (isVowel(originalText[0], false))
                return "an " + originalText;

            return "a " + originalText;
        }

        private static string applyPlural(string originalText)
        {
            if (originalText.EndsWith("f"))
                return originalText.Substring(0, originalText.Length - 1) + "ves";
            if (originalText.EndsWith("fe"))
                return originalText.Substring(0, originalText.Length - 2) + "ves";

            if (originalText.EndsWith("y") && !isVowel(originalText[originalText.Length - 2]))
                return originalText.Substring(0, originalText.Length - 1) + "ies";

            if (originalText.EndsWith("o") && !isVowel(originalText[originalText.Length - 2]))
                return originalText + "es";
            //TODO: -o plural exceptions (piano – pianos)

            if (originalText.EndsWith("ss") || originalText.EndsWith("x") ||
                originalText.EndsWith("ch") || originalText.EndsWith("sh"))
                return originalText + "es";

            //TODO: some -s and -z endings

            //TODO: irregular plural endings
            //TODO: invariant plural endings

            return originalText + "s";
        }

        private static string applyPastTense(string originalText)
        {
            if (originalText.EndsWith("e"))
                return originalText + "d";

            if (isVowel(originalText[originalText.Length - 2]) && !isVowel(originalText[originalText.Length - 1]))
                return originalText + originalText[originalText.Length - 1] + "ed";

            //If a verb ends in consonant and -y, you take off the y and add - ied.
            if (!isVowel(originalText[originalText.Length - 2]) && originalText[originalText.Length - 1] == 'y')
                return originalText.Substring(0, originalText.Length - 1) + "ied";

            //TODO: irregular past verbs

            return originalText + "ed";
        }

        private static bool isVowel(char c, bool yIsVowel = true)
        {
            return (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || (yIsVowel && c == 'y'));
        }
    }
}