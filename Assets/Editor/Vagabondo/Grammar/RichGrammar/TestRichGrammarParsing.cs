using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class TestRichGrammarParsing : BaseTestRichGrammar
    {
        [Test]
        public void TestStartRuleOption()
        {
            testGrammar("Test/testOptionStartRule", "otherRuleText");
        }

        [Test]
        public void TestShorthandSingle()
        {
            testGrammar("Test/testParsingShorthandSingle", "staticText");
        }

        [Test]
        public void TestShorthandArray()
        {
            testGrammar("Test/testParsingShorthandArray", new List<string>() { "clause1", "clause2" });
        }

        [Test]
        public void TestClausesSingle()
        {
            testGrammar("Test/testParsingClausesSingle", "staticText");
        }

        [Test]
        public void TestClausesArray()
        {
            testGrammar("Test/testParsingClausesArray", new List<string>() { "clause1", "clause2" });
        }

        [Test]
        public void TestClausesDictionary()
        {
            testGrammar("Test/testParsingClausesDictionary", new List<string>() { "clause1", "clause2" });
        }
    }
}