using Vagabondo.DataModel;
using Vagabondo.Generators;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Actions
{
    public class ShopAction : GameAction
    {
        public ShopType shopType;

        public ShopAction(ShopType shopType) : base(GameActionType.Shop)
        {
            this.shopType = shopType;
            this.title = $"Go to the local {DataUtils.EnumToStr(shopType)}";
            this.description = "You have the opportunity to buy some food, or even sell some of your own";
        }

        public override GameActionResult Perform(TravelManager travelManager)
        {
            var shopInventory = MerchandiseGenerator.GenerateInventoryFood(shopType);
            PriceEvaluator.UpdatePrices(shopInventory);
            return new ShopActionResult("", shopInventory);
        }
    }
}
