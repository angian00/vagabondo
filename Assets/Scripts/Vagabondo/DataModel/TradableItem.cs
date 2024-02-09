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

    public enum ItemCategory
    {
        FoodIngredient,
        Food,
        Drink,
        WildPlant,

        Tool,
        //...
    }

    public interface TradableItem
    {
        public string name { get; }
        public ItemCategory category { get; }
        public ItemQuality quality { get; }
        public int baseValue { get; }
        public int currentPrice { get; set; }
    }

    public class HouseholdItem : TradableItem
    {
        public string name { get; set; }
        public ItemCategory category { get; set; }
        public ItemQuality quality { get; set; }
        public int baseValue { get; set; }
        public int currentPrice { get; set; }
    }

    public enum ShopType
    {
        Tavern,
        Bakery,
        Butchery,
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
    //    public int currentPrice;
    //}
}