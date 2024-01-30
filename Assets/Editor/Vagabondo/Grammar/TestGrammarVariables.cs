using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class TestGrammarVariables : TestGrammarBase
    {
        [Test]
        public void TestVariableSimple()
        {
            testVariable("rose", "#var#", "rose");
        }

        [Test]
        public void TestVariableTwice()
        {
            testVariable("rose", "#var#_#var#", "rose_rose");
        }

        [Test]
        public void TestTwoVariables()
        {
            testVariables(new Dictionary<string, string>() { {"var1", "rose"}, {"var2", "tulip"} }, 
                "#var1#_#var2#", "rose_tulip");
        }

        [Test]
        public void TestTwoVariablesMixed()
        {
            testVariables(new Dictionary<string, string>() { {"var1", "rose"}, {"var2", "tulip"} }, 
                "#var2#_#var1#_#var2#", "tulip_rose_tulip");
        }


        protected void testVariable(string varValue, string expression, string expectedOutputText)
        {
            var variableName = "var";
            var rules = new Dictionary<string, List<string>>();
            rules.Add("varRule", new List<string>() { varValue });
            rules.Add("expression", new List<string>() { expression });
            rules.Add("origin", new List<string>() { $"#[var:#varRule#]expression#" });

            var grammar = SubstitutionGrammar.FromDictionary(rules);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(outputText, expectedOutputText);
        }

        protected void testVariables(Dictionary<string, string> varValues, string expression, string expectedOutputText)
        {
            var rules = new Dictionary<string, List<string>>();
            rules.Add("expression", new List<string>() { expression });
            
            var varRefsStr = "";
            var varKeys = varValues.Keys;
            for (var iVar = 0; iVar < varKeys.Count; iVar++) {
                var varName = varKeys[iVar];
                var varRuleName = $"varRule{iVar}";
                varRefsStr += $"[{varName}:#{varRuleName}#]";
                rules.Add(varRuleName, new List<string>() { varValues[varName] });
            }

            rules.Add("origin", new List<string>() { $"#{varRefsStr}expression#" });
            var grammar = SubstitutionGrammar.FromDictionary(rules);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(outputText, expectedOutputText);
        }
    }
}
