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
        public readonly string descriptionText;
        public readonly string resultText;

        public GameActionResult(string message) : this(message, null) { }

        public GameActionResult(string message, string resultText)
        {
            this.descriptionText = message;
            this.resultText = resultText;
        }
    }

    public class ShopActionResult : GameActionResult
    {
        public readonly ShopInfo shopInfo;
        public readonly bool skipTextResult;

        public ShopActionResult(string text, ShopInfo shopInfo, bool skipTextResult = false) : base(text)
        {
            this.shopInfo = shopInfo;
            this.skipTextResult = skipTextResult;
        }
    }
}
