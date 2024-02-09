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


    public enum ItemSubcategory
    {
        Grain,
        Vegetable,
        Legume,
        Fruit,
        Dairy,
        Cheese,
        Egg,
        Fish,
        Meat,
        Fat,
        Herb,
        Spice,

        Raw,
        Breakfast,
        Bread,
        Soup,
        MainCourse,
        Dessert,
        Preserve,

        Drink,
    }

    public class GameItem
    {
        public string name;
        public ItemCategory category;
        public ItemSubcategory subcategory;
        public ItemQuality quality;
        public int baseValue;
        public int currentPrice;

        public FoodIngredientDef definition;
    }

    public enum ShopType
    {
        Tavern,
        Bakery,
        Butchery,
    }

}