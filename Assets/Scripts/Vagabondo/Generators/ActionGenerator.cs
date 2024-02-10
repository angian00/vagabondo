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
            const float buildingGenerationProb = 0.7f;

            var newActions = new List<GameAction>();

            foreach (var building in townData.buildings)
            {
                if (UnityEngine.Random.value > buildingGenerationProb)
                    continue;

                GameAction action;
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
            const float eventGenerationProb = 0.7f;

            var candidateActionTypes = new List<GameActionType>() { };


            candidateActionTypes.Add(GameActionType.ChatLocals);

            if (townData.traits.Contains(DominionTrait.Wild))
                candidateActionTypes.Add(GameActionType.Explore);

            if (townData.traits.Contains(DominionTrait.HighCrime))
                candidateActionTypes.Add(GameActionType.ChatCriminals);


            var newActions = new List<GameAction>();
            foreach (var actionType in candidateActionTypes)
            {
                if (UnityEngine.Random.value > eventGenerationProb)
                    continue;

                var action = GameActionFactory.CreateEventAction(actionType, townData);
                newActions.Add(action);
            }

            townData.actions.AddRange(newActions);
        }
    }
}
