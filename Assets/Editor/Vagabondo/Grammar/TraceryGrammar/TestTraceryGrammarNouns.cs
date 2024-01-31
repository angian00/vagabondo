using NUnit.Framework;

namespace Vagabondo.Grammar
{
    public class TestTraceryGrammarNouns : TestTraceryGrammarBase
    {
        [Test]
        public void TestStaticRule()
        {
            var staticRule = "foo bar baz";
            var grammar = TraceryGrammar.FromSingleRule(staticRule);
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

        [Test]
        public void TestARegular()
        {
            testA("rose", "a rose");
        }

        [Test]
        public void TestAVowel()
        {
            testA("excursion", "an excursion");
        }


        private void testPlural(string noun, string expectedNounPlural)
        {
            testModifier("s", noun, expectedNounPlural);
        }

        private void testA(string noun, string expectedWithA)
        {
            testModifier("a", noun, expectedWithA);
        }

    }
}