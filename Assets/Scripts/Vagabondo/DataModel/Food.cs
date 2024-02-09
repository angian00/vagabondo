using System.Collections.Generic;
using Vagabondo.Grammar;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{


    public struct FoodIngredientDef : IGrammarNoun
    {
        public string name { get; set; }
        public bool isPluralizable { get; set; }

        public ItemSubcategory subcategory;
        public int frequency;
        public int baseValue;
        //public HashSet<Biome> compatibleBiomes;

        public GameItem Instantiate()
        {
            var item = new GameItem();
            item.name = name;
            item.category = ItemCategory.FoodIngredient;
            item.baseValue = baseValue;
            item.currentPrice = baseValue;
            item.quality = RandomUtils.RandomQuality();

            item.definition = this;

            return item;
        }
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


    public class FoodItemTemplate
    {
        public string name;
        public ItemSubcategory subcategory;
        public List<ItemSubcategory> ingredientCategories = new();
        public List<string> ingredientNames = new();
        public FoodPreparation preparation;
    }
}
