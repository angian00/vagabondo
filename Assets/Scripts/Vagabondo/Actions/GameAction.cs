using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public enum GameActionType
    {
        Forage,
        Explore,
        Tavern,
        Shop,
        Library,
        SketchyDeal,
        Quest,
    }

    public abstract class GameAction
    {
        public GameActionType type;
        public string title;
        public string description;

        public GameAction(GameActionType type)
        {
            this.type = type;
        }

        public virtual bool CanPerform(Traveler travelerData)
        {
            return true;
        }

        public virtual string GetCantPerformMessage()
        {
            return "";
        }

        public abstract GameActionResult Perform(TravelManager travelManager);
    }
}
