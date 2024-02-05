namespace Vagabondo.DataModel
{
    public enum ItemQuality
    {
        Terrible = -2,
        Poor = -1,
        Standard = 0,
        Good = 1,
        Exceptional = 2,
    }

    public interface GameItem { }

    public interface TradableItem
    {
        public string name { get; }
        public ItemQuality quality { get; }
        public int price { get; set; }
    }

    public enum ShopType
    {
        Tavern,
    }


    //public class MerchandiseItem : GameItem
    //{
    //    public enum Category
    //    {
    //        Herb,
    //        Food,
    //    }

    //    public Category category;
    //    public ItemQuality quality = ItemQuality.Standard;
    //    public int basePrice;

    //    public string text;
    //    public int price;
    //}
}