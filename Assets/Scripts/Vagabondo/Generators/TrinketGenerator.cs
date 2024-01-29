using Vagabondo.DataModel;

namespace Vagabondo.Generators
{
    public class TrinketGenerator
    {
        public static Trinket GenerateTrinket(Town townData)
        {
            var trinket = new Trinket();
            trinket.text = "a beautiful pearl necklace";

            return trinket;
        }
    }
}