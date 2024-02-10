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
                    case TownBuilding.Bakery:
                        action = new ShopAction(ShopType.Bakery, townData);
                        break;
                    case TownBuilding.Butchery:
                        action = new ShopAction(ShopType.Butchery, townData);
                        break;
                    //case TownBuilding.Emporium:
                    //    action = new ShopAction(ShopType.Emporium);
                    //    break;

                    case TownBuilding.Church:
                        action = new ChurchAction(townData);
                        break;
                    case TownBuilding.Monastery:
                        action = new MonasteryAction(townData);
                        break;
                    case TownBuilding.TownHall:
                        action = new TownHallAction(townData);
                        break;
                    case TownBuilding.Tavern:
                        action = new TavernAction(townData);
                        break;
                    case TownBuilding.Library:
                        action = new LibraryAction(townData);
                        break;

                }

                if (action != null)
                    newActions.Add(action);
            }

            townData.actions.AddRange(newActions);
        }


        private static void generateEventActions(Town townData)
        {
            var newActions = new List<GameAction>();
            //if (hasWilderness())
            newActions.Add(new ExploreAction(townData));
            //if (friendly)
            newActions.Add(new ChatLocalsAction(townData));
            //if (hasCrime())
            newActions.Add(new ChatCriminalsAction(townData));

            townData.actions.AddRange(newActions);
        }
    }
}
