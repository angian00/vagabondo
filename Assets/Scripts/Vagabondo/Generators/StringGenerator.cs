namespace Vagabondo.Generators
{
    public interface StringGenerator
    {
        public string GenerateString();
    }


    public class DummyStringGenerator : StringGenerator
    {
        private static DummyStringGenerator _instance = new DummyStringGenerator();
        public static DummyStringGenerator Instance { get => _instance; }

        private char _currChar = 'A';

        public string GenerateString()
        {
            var res = "";

            for (int i = 0; i < 6; i++)
            {
                res += _currChar.ToString();
            }

            _currChar++;

            return res;
        }
    }

}