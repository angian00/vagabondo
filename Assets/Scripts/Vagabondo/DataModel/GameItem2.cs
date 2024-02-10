using System.Collections.Generic;
using Vagabondo.Grammar;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{
    public struct IngredientDefinition : IGrammarNoun
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
            //item.isPluralizable = isPluralizable;
            item.category = ItemCategory.FoodIngredient;
            item.subcategory = subcategory;
            item.baseValue = baseValue;
            item.currentPrice = baseValue;
            item.quality = RandomUtils.RandomQuality();

            item.definition = this;

            return item;
        }
    }


    public enum Preparation
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


    public class ItemTemplate
    {
        public string name;
        public ItemSubcategory subcategory;
        public List<ItemSubcategory> ingredientCategories = new();
        public List<string> ingredientNames = new();
        public Preparation preparation;
        public int frequency = 1;

    }
}
