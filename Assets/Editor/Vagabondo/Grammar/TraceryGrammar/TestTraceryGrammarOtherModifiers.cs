using NUnit.Framework;

namespace Vagabondo.Grammar
{
    public class TestTraceryGrammarOtherModifiers : TestTraceryGrammarBase
    {

        [Test]
        public void TestCapitalize()
        {
            testModifier("capitalize", "rose", "Rose");
            testModifier("capitalize", "rose and tulip", "Rose and tulip");
        }

        [Test]
        public void TestCapitalizeAll()
        {
            testModifier("capitalizeAll", "rose", "Rose");
            testModifier("capitalizeAll", "rose and tulip", "Rose And Tulip");
        }
    }
}