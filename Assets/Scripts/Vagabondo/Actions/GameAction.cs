using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public enum GameActionType
    {
        Explore,
        Tavern,
        Shop,
        Library,

        ChatLocals,
        ChatCriminals,

        Quest,
    }

    public abstract class GameAction
    {
        public GameActionType type;
        public string title;
        public string description;
        public Town townData;

        public GameAction(GameActionType type, Town townData)
        {
            this.type = type;
            this.townData = townData;
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
