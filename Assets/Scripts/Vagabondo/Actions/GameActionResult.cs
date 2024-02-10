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
        Learn,
        Pray,
        GiveItem,
        ReceiveItem,
        LoseMoney,
    }


    public class GameActionResult
    {
        public readonly string text;
        public GameActionResult(string text)
        {
            this.text = text;
        }
    }

    public class ShopActionResult : GameActionResult
    {
        public readonly List<GameItem> shopInventory;

        public ShopActionResult(string text, List<GameItem> shopInventory) : base(text)
        {
            this.shopInventory = shopInventory;
        }
    }
}
