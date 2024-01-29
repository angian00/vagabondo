namespace Vagabondo
{
    public class MerchandiseItem
    {
        public enum Category
        {
            Herb,
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