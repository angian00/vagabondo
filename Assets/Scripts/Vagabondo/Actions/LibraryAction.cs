using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public class LibraryAction : GameAction
    {
        public LibraryAction() : base(GameActionType.Library)
        {
            this.title = "Go to the library";
            this.description = "Browse through the books of the local library";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.KnowledgeLanguages);

            return new GameActionResult($"You become more erudite than you were before");
        }
    }

}
