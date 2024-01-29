using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class TestSubstitutionGrammar
    {
        [Test]
        public void TestStaticRule()
        {
            var staticRule = "foo bar baz";
            var grammar = SubstitutionGrammar.FromSingleRule(staticRule);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(outputText, staticRule);
        }

        [Test]
        public void TestPluralRegular()
        {
            testPlural("rose", "roses");
        }

        [Test]
        public void TestPluralIrregular()
        {
            testPlural("foot", "feet");
        }

        [Test]
        public void TestPluralEndsWithY()
        {
            testPlural("lady", "ladies");
        }

        [Test]
        public void TestPluralEndsWithFE()
        {
            testPlural("knife", "knives");
        }

        [Test]
        public void TestPluralEndsWithF()
        {
            testPlural("loaf", "loaves");
        }

        [Test]
        public void TestPluralEndsWithO()
        {
            testPlural("potato", "potatoes");
        }

        [Test]
        public void TestPluralEndsWithX()
        {
            testPlural("ex", "exes");
        }

        [Test]
        public void TestPluralEndsWithCH()
        {
            testPlural("beach", "beaches");
        }


        private void testPlural(string noun, string expectedNounPlural)
        {
            var rules = new Dictionary<string, List<string>>();
            rules.Add("origin", new List<string>() { "#noun.s#" });
            rules.Add("noun", new List<string>() { noun });

            var grammar = SubstitutionGrammar.FromDictionary(rules);
            var outputText = grammar.GenerateText();

            Assert.AreEqual(outputText, expectedNounPlural);
        }

    }
}