using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class LibraryAction : TownAction
    {
        public LibraryAction(Town townData) : base(GameActionType.Library, townData)
        {
            this.title = "Go to the library";
            this.description = "Browse through the books of the local library";
        }

        public override bool isBuildingAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var effectTypes = new List<TownActionEffectType>() {
                TownActionEffectType.Learn,
                TownActionEffectType.Trade,
            };

            //TODO: influence result by stats
            var effectType = RandomUtils.RandomChoose(effectTypes);
            switch (effectType)
            {
                case TownActionEffectType.Learn:
                    return performLearn(travelManager);
                case TownActionEffectType.Trade:
                    return performTrade(travelManager);
            }

            throw new Exception($"Invalid effectType: {DataUtils.EnumToStr(effectType)}");
        }


        private TownActionResult performLearn(TravelManager travelManager)
        {
            travelManager.IncrementStat(StatId.Languages);

            var description = "You become more erudite than you were before";
            var resultText = StringUtils.BuildResultTextStat(StatId.Languages, 1);

            return new TownActionResult(description, resultText);
        }

        private TownActionResult performTrade(TravelManager travelManager)
        {
            var shopInventory = ShopInventoryGenerator.GenerateInventory(ShopType.Library, townData.shopInventorySize);
            PriceEvaluator.UpdatePrices(shopInventory, townData);

            var shopInfo = new ShopInfo("Library", townData.shopMoney, shopInventory, ShopInfo.BuyFilter[ShopType.Library]);
            return new ShopActionResult("You have the opportunity to trade books", shopInfo);
        }
    }
}
