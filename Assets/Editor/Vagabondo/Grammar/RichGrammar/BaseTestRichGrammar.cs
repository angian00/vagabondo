using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public abstract class BaseTestRichGrammar
    {
        protected void testGrammar(string grammarFile, string expectedOutput)
        {
            var grammar = new RichGrammar(grammarFile);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(expectedOutput, outputText);
        }

        protected void testGrammar(string grammarFile, List<string> expectedOutputs)
        {
            var grammar = new RichGrammar(grammarFile);
            var outputText = grammar.GenerateText();

            Assert.Contains(outputText, expectedOutputs);
        }

        protected void testGrammar(string grammarFile, string startRule, string expectedOutput)
        {
            var grammar = new RichGrammar(grammarFile);
            var outputText = grammar.GenerateText(startRule);
            Assert.AreEqual(expectedOutput, outputText);
        }

        //protected void testGrammar(string grammarFile, string startRule, string expectedOutput, string tag = null, int nRepetitions = 1)
        //{
        //    var grammar = new RichGrammar(grammarFile);
        //    var tags = new HashSet<string>() { tag };
        //    if (tag != null)
        //        tags.Add(tag);
        //    for (var i = 0; i < nRepetitions; i++)
        //    {
        //        var outputText = grammar.GenerateText(startRule, tags);
        //        Assert.AreEqual(expectedOutput, outputText);
        //    }
        //}

        protected void testGrammarFrequency(string grammarFile, string startRule, string expectedOutput, string tag = null,
            int nRepetitions = 1, float minFrequency = 0, float maxFrequency = 1)
        {
            var grammar = new RichGrammar(grammarFile);
            var tags = new HashSet<string>() { };
            if (tag != null)
                tags.Add(tag);

            int nMatches = 0;
            for (var i = 0; i < nRepetitions; i++)
            {
                var outputText = grammar.GenerateText(startRule, tags);
                if (outputText == expectedOutput)
                    nMatches++;
            }

            var frequency = ((float)nMatches) / nRepetitions;
            Assert.GreaterOrEqual(frequency, minFrequency);
            Assert.LessOrEqual(frequency, maxFrequency);
        }
    }
}