using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using static Vagabondo.Grammar.GrammarFactory;

namespace Vagabondo.Actions
{
    public class SketchyDealAction : GameAction
    {
        public static int dealCost = 50;

        private Town townData;
        public SketchyDealAction(Town townData) : base(GameActionType.SketchyDeal)
        {
            this.townData = townData;
            this.title = "Explore the streets";
            this.description = "Look for a bargain deal in the seediest part of the town";
        }

        public override bool CanPerform(Traveler travelerData)
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
            var resultText = GetGrammar(GrammarId.SketchyDeal).GenerateText();
            return new ItemAcquiredActionResult(resultText, trinket);
        }
    }

}
