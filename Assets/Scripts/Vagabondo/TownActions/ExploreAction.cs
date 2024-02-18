using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class ExploreAction : TownAction
    {
        public ExploreAction(Town townData) : base(GameActionType.Explore, townData)
        {
            this.title = "Go for a hike";
            this.description = "Spend some time getting acquainted with the countryside";
        }

        public override bool isEventAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Learn,
                TownActionEffectType.ReceiveItem,
                TownActionEffectType.Injury,
            };


            //TODO: influence result by Knowledge.Nature
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Learn:
                    return performLearn(travelManager);
                case TownActionEffectType.ReceiveItem:
                    return performReceiveItem(travelManager);
                case TownActionEffectType.Injury:
                    return performInjury(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private TownActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Nature);

            var description = "You learn some interesting facts about the local wilderness";
            var resultText = StringUtils.BuildResultTextStat(StatId.Nature, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performReceiveItem(TravelManager travelManager)
        {
            var plant = ShopInventoryGenerator.GenerateItem(ItemCategory.WildPlant);

            travelManager.AddItem(plant);

            var description = "You gather some useful herbs";
            var resultText = StringUtils.BuildResultTextItem(plant, true);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performInjury(TravelManager travelManager)
        {
            const int maxInjury = 8;

            var injuryAmount = UnityEngine.Random.Range(1, maxInjury);
            travelManager.AddHealth(-injuryAmount);

            var animalName = "bear"; //TODO: generate animal names by biome

            var description = $"You enconter a dangerous {animalName} which attacks you!";
            var resultText = StringUtils.BuildResultTextHealth(-injuryAmount);

            return new TownActionResult(description, resultText);
        }
    }
}
