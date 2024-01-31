using NUnit.Framework;

namespace Vagabondo.Grammar
{
    public class TestRichGrammarModifiers : BaseTestRichGrammar
    {
        [Test]
        public void TestIndefiniteArticle()
        {
            testGrammar("Test/testModifiers", "indefiniteA", "a rose");
            testGrammar("Test/testModifiers", "indefiniteAN", "an orchid");
        }

        [Test]
        public void TestPlural()
        {
            testGrammar("Test/testModifiers", "pluralS", "carrots");
            testGrammar("Test/testModifiers", "pluralIrregular", "feet");
            testGrammar("Test/testModifiers", "pluralFVES", "knives");
            testGrammar("Test/testModifiers", "pluralFVES2", "loaves");
            testGrammar("Test/testModifiers", "pluralOES", "potatoes");
            testGrammar("Test/testModifiers", "pluralXES", "exes");
            testGrammar("Test/testModifiers", "pluralCHES", "beaches");
        }

        [Test]
        public void TestPastTense()
        {
            testGrammar("Test/testModifiers", "simplePastED", "helped");
            testGrammar("Test/testModifiers", "simplePastIrregular", "ate");
            testGrammar("Test/testModifiers", "simplePastD", "danced");
            testGrammar("Test/testModifiers", "simplePastIED", "tried");
        }


        [Test]
        public void TestCapitalization()
        {
            testGrammar("Test/testModifiers", "capitalizeSingle", "Rose");
            testGrammar("Test/testModifiers", "capitalizePhrase", "Rose and tulip");
            testGrammar("Test/testModifiers", "capitalizeAllSingle", "Rose");
            testGrammar("Test/testModifiers", "capitalizeAllPhrase", "Rose And Tulip");
        }

    }
}