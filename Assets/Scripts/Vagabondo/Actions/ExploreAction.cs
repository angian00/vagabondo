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

        public override GameActionResult Perform(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.KnowledgeNature);

            return new GameActionResult($"You learn some interesting facts about the local wilderness");
        }
    }
}
