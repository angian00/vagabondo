using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vagabondo.Grammar
{
    class RichGrammarRule
    {
        public Dictionary<string, int> clauses = new();
        public Dictionary<string, string> variables = new();
        public HashSet<string> tags = new();
    }

    public partial class RichGrammar : SubstitutionGrammar
    {
        private const string defaultStartRule = "origin";
        private string startRule = defaultStartRule;
        private Dictionary<string, RichGrammarRule> rules = new();

        private RichGrammar() { }

        public RichGrammar(string filename)
        {
            var fileObj = Resources.Load<TextAsset>($"Data/Grammars/{filename}");

            dynamic parsedGrammar = JsonConvert.DeserializeObject(fileObj.text);

            if (parsedGrammar.startRule != null)
                this.startRule = parsedGrammar.startRule;

            var rulesDict = ((JObject)parsedGrammar.rules).ToObject<Dictionary<string, dynamic>>();
            foreach (string ruleName in rulesDict.Keys)
            {
                var ruleObj = rulesDict[ruleName];

                rules.Add(ruleName, parseRule(ruleObj));
            }
        }

        private RichGrammarRule parseRule(dynamic ruleObj)
        {
            RichGrammarRule rule = new();

            if (ruleObj.GetType() == typeof(string))
            {
                rule.clauses = new Dictionary<string, int>() { { (string)ruleObj, 1 } };
            }
            else if (ruleObj.GetType() == typeof(JArray))
            {
                rule.clauses = parseValuesArray((JArray)ruleObj).ToDictionary(clauseText => clauseText, clauseText => 1);
            }
            else if (ruleObj.GetType() == typeof(JObject))
            {
                var ruleProperties = ((JObject)ruleObj).ToObject<Dictionary<string, dynamic>>();
                foreach (var rulePropertyKey in ruleProperties.Keys)
                {
                    var rulePropertyValue = ruleProperties[rulePropertyKey];

                    if (rulePropertyKey == "clauses")
                    {
                        if (rulePropertyValue.GetType() == typeof(string))
                            rule.clauses = new Dictionary<string, int>() { { (string)rulePropertyValue, 1 } };
                        else if (rulePropertyValue.GetType() == typeof(JArray))
                            rule.clauses = parseValuesArray((JArray)rulePropertyValue).ToDictionary(clauseText => clauseText, clauseText => 1);
                        else if (rulePropertyValue.GetType() == typeof(JObject))
                            rule.clauses = parseRuleClausesDictionary(((JObject)rulePropertyValue).ToObject<Dictionary<string, dynamic>>());
                        else
                            throw new NotImplementedException($"Cannot parse rule clauses type: {rulePropertyValue.GetType()}");
                    }
                    else if (rulePropertyKey == "variables")
                    {
                        rule.variables = ((JObject)rulePropertyValue).ToObject<Dictionary<string, string>>();
                    }
                    else if (rulePropertyKey == "tags")
                    {
                        if (rulePropertyValue.GetType() == typeof(string))
                            rule.tags = new HashSet<string>() { (string)rulePropertyValue };
                        else if (rulePropertyValue.GetType() == typeof(JArray))
                            rule.tags = new HashSet<string>(parseValuesArray((JArray)rulePropertyValue));
                    }
                    else
                        Debug.LogWarning($"Unknown parsed rule property: {rulePropertyKey}");
                }
            }
            else
                throw new NotImplementedException($"Cannot parse rule type: {ruleObj.GetType()}");

            return rule;
        }

        private List<string> parseValuesArray(JArray arr)
        {
            List<string> result = new();

            foreach (var itemObj in arr)
            {
                if (itemObj.GetType() == typeof(JValue))
                    result.Add(((JValue)itemObj).ToString());
                else
                    throw new NotImplementedException($"Cannot parse array item type: {itemObj.GetType()}");
            }

            return result;
        }

        private Dictionary<string, int> parseRuleClausesDictionary(Dictionary<string, dynamic> clausesDict)
        {
            Dictionary<string, int> clauses = new();

            foreach (var clauseText in clausesDict.Keys)
            {
                var weight = (int)clausesDict[clauseText];
                clauses.Add(clauseText, weight);
            }

            return clauses;
        }
    }
}