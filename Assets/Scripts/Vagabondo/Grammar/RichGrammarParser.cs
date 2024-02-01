using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vagabondo.Grammar
{
    class RichGrammarRule
    {
        public Dictionary<string, int> clauses = new();
        public Dictionary<string, string> variables = new();
        public HashSet<string> tags = new();
    }

    class ParsedGrammar
    {
        public string startRule;
        public Dictionary<string, JToken> rules;
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

            var preprocessedText = CommentRemover.RemoveComments(fileObj.text);
            var parsedGrammar = JsonConvert.DeserializeObject<ParsedGrammar>(preprocessedText);

            if (parsedGrammar.startRule != null)
                this.startRule = parsedGrammar.startRule;

            foreach (string ruleName in parsedGrammar.rules.Keys)
            {
                var ruleObj = parsedGrammar.rules[ruleName];
                rules.Add(ruleName, parseRule(ruleObj));
            }
        }

        private RichGrammarRule parseRule(JToken ruleObj)
        {
            RichGrammarRule rule = new();

            if (ruleObj.GetType() == typeof(JValue))
            {
                rule.clauses = new Dictionary<string, int>() { { (string)ruleObj, 1 } };
            }
            else if (ruleObj.GetType() == typeof(JArray))
            {
                rule.clauses = parseValuesArrayAsDictionary((JArray)ruleObj);
            }
            else if (ruleObj.GetType() == typeof(JObject))
            {
                var ruleProperties = ((JObject)ruleObj).ToObject<Dictionary<string, JToken>>();
                foreach (var rulePropertyKey in ruleProperties.Keys)
                {
                    var rulePropertyValue = ruleProperties[rulePropertyKey];

                    if (rulePropertyKey == "clauses")
                    {
                        if (rulePropertyValue.GetType() == typeof(string))
                            rule.clauses = new Dictionary<string, int>() { { (string)rulePropertyValue, 1 } };
                        else if (rulePropertyValue.GetType() == typeof(JArray))
                            rule.clauses = parseValuesArrayAsDictionary((JArray)rulePropertyValue);
                        else if (rulePropertyValue.GetType() == typeof(JObject))
                            rule.clauses = parseRuleClausesDictionary(((JObject)rulePropertyValue).ToObject<Dictionary<string, JToken>>());
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
                            rule.tags = new HashSet<string>(parseValuesArrayAsHashSet((JArray)rulePropertyValue));
                    }
                    else
                        throw new Exception($"Unknown parsed rule property: {rulePropertyKey}");
                }
            }
            else
                throw new NotImplementedException($"Cannot parse rule type: {ruleObj.GetType()}");

            return rule;
        }

        private HashSet<string> parseValuesArrayAsHashSet(JArray arr)
        {
            var result = new List<string>();

            foreach (var itemObj in arr)
            {
                if (itemObj.GetType() == typeof(JValue))
                    result.Add(((JValue)itemObj).ToString());
                else
                    throw new NotImplementedException($"Cannot parse array item type: {itemObj.GetType()}");
            }

            return new HashSet<string>(result);
        }

        private Dictionary<string, int> parseValuesArrayAsDictionary(JArray arr)
        {
            var result = new Dictionary<string, int>();

            foreach (var itemObj in arr)
            {
                if (itemObj.Type == JTokenType.Comment)
                {
                    Debug.LogWarning("skipping comment");
                    continue;
                }

                if (itemObj.GetType() == typeof(JValue))
                    result.Add(((JValue)itemObj).ToString(), 1);
                else
                    throw new NotImplementedException($"Cannot parse array item type: {itemObj.GetType()}");
            }

            return result;
        }

        private Dictionary<string, int> parseRuleClausesDictionary(Dictionary<string, JToken> clausesDict)
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