using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class ExploreAction : GameAction
    {
        public ExploreAction(Town townData) : base(GameActionType.Explore, townData)
        {
            this.title = "Go for a hike";
            this.description = "Spend some time getting acquainted with the countryside";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Learn,
                GameActionEffectType.ReceiveItem,
                GameActionEffectType.Injury,
            };


            //TODO: influence result by Knowledge.Nature
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.Learn:
                    return performLearn(travelManager);
                case GameActionEffectType.ReceiveItem:
                    return performReceiveItem(travelManager);
                case GameActionEffectType.Injury:
                    return performInjury(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private GameActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Nature);

            return new GameActionResult($"You learn some interesting facts about the local wilderness");
        }

        private GameActionResult performReceiveItem(TravelManager travelManager)
        {
            var plant = MerchandiseGenerator.GenerateItem(ItemCategory.WildPlant);

            travelManager.AddItem(plant);

            return new GameActionResult($"You find some useful <style=C1>{plant.name}</style>");
        }

        private GameActionResult performInjury(TravelManager travelManager)
        {
            const int maxInjury = 8;

            var injuryAmount = UnityEngine.Random.Range(1, maxInjury);
            travelManager.AddHealth(-injuryAmount);

            var animalName = "bear"; //TODO: generate animal names by biome

            return new GameActionResult($"You enconter a dangerous {animalName} which attacks you!");
        }
    }
}
