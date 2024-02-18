using System;
using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.TownActions
{
    public class ShopAction : TownAction
    {
        public ShopType shopType;

        public ShopAction(ShopType shopType, Town townData) : base(GameActionType.Shop, townData)
        {
            this.shopType = shopType;
            this.title = $"Go to the local {DataUtils.EnumToStr(shopType).ToLower()}";
            this.description = "You have the opportunity to buy some merchandise, or even sell some of your own";
        }

        public override bool isShopAction() => true;

        public override TownActionResult Perform(TravelManager travelManager)
        {
            var shopName = DataUtils.EnumToStr(shopType);

            var shopInventory = MerchandiseGenerator.GenerateInventory(shopType);
            PriceEvaluator.UpdatePrices(shopInventory, townData);

            Predicate<GameItem> canBuy = ShopInfo.BuyFilter.ContainsKey(shopType) ? ShopInfo.BuyFilter[shopType] : null;

            var shopInfo = new ShopInfo(shopName, shopInventory, canBuy);
            return new ShopActionResult("", shopInfo, true);
        }
    }
}
