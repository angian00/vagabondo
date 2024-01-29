using Vagabondo.Generators;
using static Vagabondo.Grammar.GrammarFactory;

namespace Vagabondo
{
    public class SketchyDealAction : GameAction
    {
        public static int dealCost = 50;

        private TownData townData;
        public SketchyDealAction(TownData townData) : base(GameActionType.SketchyDeal)
        {
            this.townData = townData;
            this.title = "Explore the streets";
            this.description = "Look for a bargain deal in the seediest part of the town";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return (travelerData.money >= dealCost);
        }

        public override string GetCantPerformMessage()
        {
            return $"You must have at least {dealCost}$ to spend in the street";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            travelManager.AddMoney(-dealCost);

            var trinket = TrinketGenerator.GenerateTrinket(townData);
            travelManager.AddTrinket(trinket);

            //TODO: trinket discovery UI
            var resultText = CreateGrammar(GrammarId.SketchyDeal).GenerateText();
            return new ItemAcquiredActionResult(resultText, trinket);
        }
    }

}
