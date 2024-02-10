using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class LibraryAction : GameAction
    {
        public LibraryAction(Town townData) : base(GameActionType.Library, townData)
        {
            this.title = "Go to the library";
            this.description = "Browse through the books of the local library";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<GameActionEffectType>() {
                GameActionEffectType.Learn,
                GameActionEffectType.Trade,
            };

            //TODO: influence result by stats
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case GameActionEffectType.Learn:
                    return performLearn(travelManager);
                case GameActionEffectType.Trade:
                    return performTrade(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private GameActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Languages);
            return new GameActionResult($"You become more erudite than you were before");
        }

        private GameActionResult performTrade(TravelManager travelManager)
        {
            var shopInventory = MerchandiseGenerator.GenerateInventory(ShopType.Library);
            PriceEvaluator.UpdatePrices(shopInventory);
            return new ShopActionResult("You have the opportunity to trade books", shopInventory);
        }
    }
}
