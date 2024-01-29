using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public class ExploreAction : GameAction
    {
        public ExploreAction() : base(GameActionType.Explore)
        {
            this.title = "Go for a hike";
            this.description = "Spend some time getting acquainted with the countryside";
        }

        public override bool CanPerform(Traveler travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            travelManager.IncrementKnowledge(KnowledgeType.Nature);

            return new TextActionResult($"You learn some interesting facts about the local wilderness");
        }
    }
}
