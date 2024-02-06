using System.Collections.Generic;
using Vagabondo.Actions;
using Vagabondo.DataModel;

namespace Vagabondo.Generators
{
    public class ActionGenerator
    {
        public static void GenerateActions(Town townData)
        {
            generateBuildingActions(townData);
            generateEventActions(townData);
        }

        private static void generateBuildingActions(Town townData)
        {
            var newActions = new List<GameAction>();

            foreach (var building in townData.buildings)
            {
                GameAction action = null;
                switch (building)
                {
                    case TownBuilding.Tavern:
                        action = new TavernAction();
                        break;

                    case TownBuilding.Library:
                        action = new LibraryAction();
                        break;

                    case TownBuilding.Bakery:
                        action = new ShopAction(ShopType.Bakery);
                        break;
                        //Emporium,
                        //Butchery,

                        //case TownBuilding.Church:
                        //    action = new ChurchAction();
                        //    break;

                        //case TownBuilding.Monastery:
                        //    action = new MonasteryAction();
                        //    break;
                        //case TownBuilding.TownHall:
                        //    action = new TownHallAction();
                        //    break;

                }

                if (action != null)
                    newActions.Add(action);
            }

            townData.actions.AddRange(newActions);
        }


        private static void generateEventActions(Town townData)
        {
            var newActions = new List<GameAction>();
            newActions.Add(new ForageAction(townData.biome));
            //if (hasWilderness())
            newActions.Add(new ExploreAction());
            //if (hasCrime())
            newActions.Add(new SketchyDealAction(townData));
            //if (friendly)
            newActions.Add(new FoodGiftAction(townData));

            townData.actions.AddRange(newActions);
        }
    }
}
