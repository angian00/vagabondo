using NUnit.Framework;

namespace Vagabondo.Grammar
{
    public class TestTraceryGrammarVerbs : TestTraceryGrammarBase
    {
        [Test]
        public void TestPastRegular()
        {
            testSimplePast("help", "helped");
        }

        [Test]
        public void TestPastIrregular()
        {
            testSimplePast("eat", "ate");
        }

        [Test]
        public void TestPastEndsWithE()
        {
            testSimplePast("dance", "danced");
        }

        [Test]
        public void TestPastEndsWithY()
        {
            testSimplePast("try", "tried");
        }

        private void testSimplePast(string verb, string expectedSimplePast)
        {
            testModifier("ed", verb, expectedSimplePast);
        }

    }
}