using Vagabondo.Generators;
using static Vagabondo.Grammar.GrammarFactory;

namespace Vagabondo
{
    public enum GameActionType
    {
        Forage,
        Tavern,
        SketchyDeal,
        Explore,
        Library,
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
        public abstract string Perform(TravelManager travelManager);
    }

    public class ForageAction : GameAction
    {
        public Biome biome;

        public ForageAction(Biome biome) : base(GameActionType.Forage)
        {
            this.biome = biome;
            this.title = "Forage";
            this.description = "Walk around the countryside looking for useful herbs";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

        public override string Perform(TravelManager travelManager)
        {
            var merchItem = MerchandiseGenerator.GenerateHerb(biome);
            travelManager.AddMerchandiseItem(merchItem);

            return $"You gather some {merchItem.text}";
        }
    }

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

        public override string Perform(TravelManager travelManager)
        {
            travelManager.IncrementKnowledge(KnowledgeType.Politics);
            travelManager.AddMoney(-tavernCost);

            return $"You learn some interesting facts about the local government";
        }
    }

    public class SketchyDealAction : GameAction
    {
        public static int dealCost = 50;

        private TownData townData;
        public SketchyDealAction(TownData townData) : base(GameActionType.SketchyDeal)
        {
            this.townData = townData;
            this.title = "Explore the streets";
            this.description = "Look for a bargain deal in the seediest part of the town";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return (travelerData.money >= dealCost);
        }

        public override string GetCantPerformMessage()
        {
            return $"You must have at least {dealCost}$ to spend in the street";
        }

        public override string Perform(TravelManager travelManager)
        {
            travelManager.AddMoney(-dealCost);

            //return $"You learn some interesting facts about the local government";
            var trinket = TrinketGenerator.GenerateTrinket(townData);
            travelManager.AddTrinket(trinket);

            //TODO: trinket discovery UI
            return CreateGrammar(GrammarId.SketchyDeal).GenerateText();
            //return $"You got a {trinket.text} in exchange for your money. Did you get a good deal? Hmmm..."; 
        }
    }

    public class ExploreAction : GameAction
    {
        public ExploreAction() : base(GameActionType.Explore)
        {
            this.title = "Go for a hike";
            this.description = "Spend some time getting acquainted with the countryside";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

        public override string Perform(TravelManager travelManager)
        {
            travelManager.IncrementKnowledge(KnowledgeType.Nature);

            return $"You learn some interesting facts about the local wilderness";
        }
    }


    public class LibraryAction : GameAction
    {
        public LibraryAction() : base(GameActionType.Library)
        {
            this.title = "Go to the library";
            this.description = "Browse through the books of the local library";
        }

        public override bool CanPerform(TravelerData travelerData)
        {
            return true;
        }

        public override string GetCantPerformMessage()
        {
            return "";
        }

        public override string Perform(TravelManager travelManager)
        {
            travelManager.IncrementKnowledge(KnowledgeType.Languages);

            return $"You become more erudite than you were before";
        }
    }

}
