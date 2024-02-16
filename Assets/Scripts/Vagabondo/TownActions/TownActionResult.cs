using Vagabondo.DataModel;

namespace Vagabondo.TownActions
{
    public enum TownActionEffectType
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


    public class TownActionResult
    {
        public readonly string descriptionText;
        public readonly string resultText;

        public TownActionResult(string message) : this(message, null) { }

        public TownActionResult(string message, string resultText)
        {
            this.descriptionText = message;
            this.resultText = resultText;
        }
    }

    public class ShopActionResult : TownActionResult
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
