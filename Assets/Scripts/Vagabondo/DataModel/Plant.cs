namespace Vagabondo.DataModel
{

    public class Plant : TradableItem
    {
        public string name { get; set; }
        public ItemCategory category { get => ItemCategory.WildPlant; }
        public ItemQuality quality { get; set; }
        public int baseValue { get; set; }
        public int currentPrice { get; set; }
    }

}