namespace Vagabondo
{
    public enum GameActionType
    {
        Forage,
        Explore,
        Tavern,
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

        public abstract bool CanPerform(TravelerData travelerData);
        public abstract string GetCantPerformMessage();
        public abstract GameActionResult Perform(TravelManager travelManager);
    }
}
