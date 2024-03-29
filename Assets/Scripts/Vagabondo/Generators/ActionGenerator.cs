using System.Collections.Generic;
using Vagabondo.TownActions;
using Vagabondo.DataModel;
using Vagabondo.Managers;

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
            var newActions = new List<TownAction>();

            foreach (var building in townData.buildings)
            {
                if (UnityEngine.Random.value > GameParams.Instance.buildingActionProbability)
                    continue;

                TownAction action;
                switch (building)
                {
                    case TownBuilding.Bakery:
                        action = new ShopAction(ShopType.Bakery, townData);
                        break;

                    case TownBuilding.Butchery:
                        action = new ShopAction(ShopType.Butchery, townData);
                        break;

                    case TownBuilding.Farm:
                        action = new ShopAction(ShopType.Farm, townData);
                        break;

                    //case TownBuilding.Emporium:
                    //    action = new ShopAction(ShopType.Emporium);
                    //    break;

                    default:
                        action = GameActionFactory.CreateBuildingAction(building, townData);
                        break;

                }

                newActions.Add(action);
            }

            townData.actions.AddRange(newActions);
        }


        private static void generateEventActions(Town townData)
        {
            var candidateActionTypes = new List<GameActionType>() { };


            candidateActionTypes.Add(GameActionType.ChatLocals);

            if (townData.traits.Contains(TownTrait.Wild))
                candidateActionTypes.Add(GameActionType.Explore);

            if (townData.traits.Contains(TownTrait.HighCrime))
                candidateActionTypes.Add(GameActionType.ChatCriminals);


            var newActions = new List<TownAction>();
            foreach (var actionType in candidateActionTypes)
            {
                if (UnityEngine.Random.value > GameParams.Instance.eventActionProbability)
                    continue;

                var action = GameActionFactory.CreateEventAction(actionType, townData);
                newActions.Add(action);
            }

            townData.actions.AddRange(newActions);
        }
    }
}
