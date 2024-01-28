namespace Vagabondo.Generators
{
    public class TrinketGenerator
    {
        public static Trinket GenerateTrinket(TownData townData)
        {
            var trinket = new Trinket();
            trinket.text = "a beautiful pearl necklace";

            return trinket;
        }
    }
}