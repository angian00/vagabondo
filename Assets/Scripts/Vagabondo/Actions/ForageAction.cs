using Vagabondo.Generators;
using static Vagabondo.Grammar.GrammarFactory;

namespace Vagabondo
{
    public class ForageAction : GameAction
    {
        public Biome biome;

        public ForageAction(Biome biome) : base(GameActionType.Forage)
        {
            this.biome = biome;
            this.title = "Forage";
            this.description = "Walk around the countryside looking for useful herbs";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var merchItem = MerchandiseGenerator.GenerateHerb(biome);
            travelManager.AddMerchandiseItem(merchItem);

            return new TextActionResult($"You gather some {merchItem.text}");
        }
    }

}
