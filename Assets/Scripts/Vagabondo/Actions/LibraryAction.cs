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
            travelManager.IncrementKnowledge(KnowledgeType.Languages);

            return new TextActionResult($"You become more erudite than you were before");
        }
    }

}
