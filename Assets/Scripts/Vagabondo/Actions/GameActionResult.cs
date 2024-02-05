using System.Collections.Generic;
using Vagabondo.DataModel;

namespace Vagabondo.Actions
{
    public enum GameActionEffectType
    {
        Trade,
        Gossip,
        MakeFriends,
        MakeEnemies,
        Injury,
    }


    public class GameActionResult
    {
        public readonly string text;
        public GameActionResult(string text)
        {
            this.text = text;
        }
    }

    public class ItemAcquiredActionResult : GameActionResult
    {
        public GameItem item;

        public ItemAcquiredActionResult(string text, GameItem item) : base(text)
        {
            this.item = item;
        }
    }


    public class ShopActionResult : GameActionResult
    {
        public readonly List<TradableItem> shopInventory;

        public ShopActionResult(string text, List<TradableItem> shopInventory) : base(text)
        {
            this.shopInventory = shopInventory;
        }
    }
}
