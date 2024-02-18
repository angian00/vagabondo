using Vagabondo.Utils;

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
        Book,
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

    public enum UseVerb
    {
        None,
        Eat,
        Drink,

        //Read,
        //...
    }

    public class GameItem
    {
        public string name;
        public ItemCategory category;
        public ItemSubcategory subcategory;
        public ItemQuality quality;
        public int baseValue;
        public int currentPrice;
        public int nutrition;
        public UseVerb useVerb = UseVerb.None;
        public float abundance;

        public string extendedName
        {
            get
            {
                var qualityStr = "";
                if (quality < ItemQuality.Standard)
                    qualityStr = $"\n<style=BAD>[{DataUtils.EnumToStr(quality).ToLower()}]</style>";
                else if (quality > ItemQuality.Standard)
                    qualityStr = $"\n<style=GOOD>[{DataUtils.EnumToStr(quality).ToLower()}]</style>";

                return $"{name}{qualityStr}";
            }
        }

        public IngredientDefinition definition;
    }

}