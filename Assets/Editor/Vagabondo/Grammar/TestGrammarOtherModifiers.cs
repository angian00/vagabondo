using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class TestGrammarOtherModifiers : TestGrammarBase
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
            testModifier("capitalize", "rose", "Rose");
            testModifier("capitalize", "rose and tulip", "Rose And Tulip");
        }
    }
}