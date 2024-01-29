using Vagabondo.Generators;
using static Vagabondo.Grammar.GrammarFactory;

namespace Vagabondo
{
    public class TavernAction : GameAction
    {
        public static int tavernCost = 10;

        public TavernAction() : base(GameActionType.Tavern)
        {
            this.title = "Go to the tavern";
            this.description = "Spend some cash in the local watering hole";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return (travelerData.money >= tavernCost);
        }

        public override string GetCantPerformMessage()
        {
            return $"You must have at least {tavernCost}$ to be served in the tavern";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            travelManager.IncrementKnowledge(KnowledgeType.Politics);
            travelManager.AddMoney(-tavernCost);

            return new TextActionResult($"You learn some interesting facts about the local government");
        }
    }
}
