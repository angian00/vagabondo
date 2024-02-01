namespace Vagabondo.DataModel
{
    public interface GameItem { }

    public class MerchandiseItem : GameItem
    {
        public enum Category
        {
            Herb,
            Food,
        }

        public enum Quality
        {
            Low,
            Standard,
            High,
            Exceptional,
        }

        public Category category;
        public Quality quality = Quality.Standard;
        public int basePrice;

        public string text;
        public int price;
    }
}