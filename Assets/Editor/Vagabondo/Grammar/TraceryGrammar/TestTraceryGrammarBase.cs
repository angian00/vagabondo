using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public abstract class TestTraceryGrammarBase
    {
        protected void testModifier(string modifier, string inputText, string expectedOutputText)
        {
            var rules = new Dictionary<string, List<string>>();
            rules.Add("symbol", new List<string>() { inputText });
            rules.Add("origin", new List<string>() { $"#symbol.{modifier}#" });

            var grammar = TraceryGrammar.FromDictionary(rules);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(expectedOutputText, outputText);
        }

        protected void testModifiers(List<string> modifiers, string inputText, string expectedOutputText)
        {
            var rules = new Dictionary<string, List<string>>();
            rules.Add("symbol", new List<string>() { inputText });

            var symbolExpr = "symbol";
            foreach (var modifier in modifiers)
                symbolExpr += $".{modifier}";

            rules.Add("origin", new List<string>() { $"#{symbolExpr}#" });

            var grammar = TraceryGrammar.FromDictionary(rules);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(expectedOutputText, outputText);
        }
    }
}