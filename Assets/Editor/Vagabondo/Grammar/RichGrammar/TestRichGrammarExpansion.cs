using NUnit.Framework;

namespace Vagabondo.Grammar
{
    public class TestRichGrammarExpansion : BaseTestRichGrammar
    {
        [Test]
        public void TestNestedRules()
        {
            testGrammar("Test/testExpansion", "singleNesting", "text");
            testGrammar("Test/testExpansion", "doubleNesting", "text");
        }

        [Test]
        public void TestVariables()
        {
            testGrammar("Test/testExpansion", "singleVariable", "text");
            testGrammar("Test/testExpansion", "singleVariableTwice", "text_text");
            testGrammar("Test/testExpansion", "twoVariables", "text1_text2");
            testGrammar("Test/testExpansion", "nestedVariables", "text");
        }

        [Test]
        public void TestTags()
        {
            testGrammarFrequency("Test/testExpansion", "tagging", "textA", tag: "tagA", nRepetitions: 10, minFrequency: 1);
            testGrammarFrequency("Test/testExpansion", "tagging", "textB", tag: "tagB", nRepetitions: 10, minFrequency: 1);
        }

        [Test]
        public void TestWeights()
        {
            testGrammarFrequency("Test/testExpansion", "weighting", "textA", nRepetitions: 10, minFrequency: 0.7f);
            testGrammarFrequency("Test/testExpansion", "weighting", "textB", nRepetitions: 10, maxFrequency: 0.3f);
        }
    }
}
