using System.Collections.Generic;
using Vagabondo.Grammar;

namespace Vagabondo.DataModel
{
    public enum FoodIngredientCategory
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
        Drink,
    }

    public struct FoodIngredientDef : IGrammarNoun
    {
        public string name { get; set; }
        public bool isPluralizable { get; set; }

        public FoodIngredientCategory category;
        public int frequency;
        public int baseValue;
        public HashSet<Biome> compatibleBiomes;
    }

    public class FoodIngredient : TradableItem
    {
        public FoodIngredientDef definition;
        public string name { get => definition.name; }
        public ItemCategory category { get => ItemCategory.FoodIngredient; }
        public ItemQuality quality { get; set; }
        public int baseValue { get => definition.baseValue; }
        public int currentPrice { get; set; }
    }

    public enum FoodPreparation
    {
        None,
        Mix,
        Boil,
        Bake,
        Stew,
        Roast,
        Fry,
        Simmer,
        Preserve,
    }


    public enum FoodItemCategory
    {
        Raw,
        Breakfast,
        Bread,
        Soup,
        MainCourse,
        Dessert,
        Preserve,
        Drink,
    }

    public class FoodItemTemplate
    {
        public string name;
        public FoodItemCategory category;
        public List<FoodIngredientCategory> ingredientCategories = new();
        public List<string> ingredientNames = new();
        public FoodPreparation preparation;
    }


    public class FoodItem : TradableItem
    {
        public string name { get; set; }
        public ItemCategory category { get => foodCategory == FoodItemCategory.Drink ? ItemCategory.Drink : ItemCategory.Food; }

        public FoodItemCategory foodCategory;
        public List<FoodIngredient> ingredients = new();
        public FoodPreparation preparation;
        public ItemQuality preparationQuality;
        public int baseValue { get; set; }

        public ItemQuality quality { get; set; }
        public int currentPrice { get; set; }

        //FUTURE: perishability
    }
}