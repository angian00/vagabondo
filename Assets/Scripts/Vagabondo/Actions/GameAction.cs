using Vagabondo.DataModel;
using Vagabondo.Managers;

namespace Vagabondo.Actions
{
    public enum GameActionType
    {
        Church,
        TownHall,
        Tavern,
        Library,

        Shop,

        Explore,
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


    public class GameActionFactory
    {
        public static GameAction CreateEventAction(GameActionType type, Town townData)
        {
            switch (type)
            {
                case GameActionType.Explore:
                    return new ExploreAction(townData);
                case GameActionType.ChatLocals:
                    return new ChatLocalsAction(townData);
                case GameActionType.ChatCriminals:
                    return new ChatCriminalsAction(townData);
            }

            throw new System.Exception($"Invalid GameActionType: {type}");
        }

        public static GameAction CreateBuildingAction(TownBuilding building, Town townData)
        {
            switch (building)
            {
                case TownBuilding.Church:
                    return new ChurchAction(townData);
                case TownBuilding.Monastery:
                    return new MonasteryAction(townData);
                case TownBuilding.TownHall:
                    return new TownHallAction(townData);
                case TownBuilding.Tavern:
                    return new TavernAction(townData);
                case TownBuilding.Library:
                    return new LibraryAction(townData);
            }

            throw new System.Exception($"Invalid GameActionType: {building}");
        }
    }

}
