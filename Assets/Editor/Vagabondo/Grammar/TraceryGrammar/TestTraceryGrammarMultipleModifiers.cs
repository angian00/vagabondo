using NUnit.Framework;
using System.Collections.Generic;

namespace Vagabondo.Grammar
{
    public class TestTraceryGrammarMultipleModifiers : TestTraceryGrammarBase
    {
        [Test]
        public void TestCapitalizeA()
        {
            testModifiers(new List<string>() { "capitalize", "a" }, "rose", "a Rose");
            testModifiers(new List<string>() { "a", "capitalize" }, "rose", "A rose");
        }
    }
}
