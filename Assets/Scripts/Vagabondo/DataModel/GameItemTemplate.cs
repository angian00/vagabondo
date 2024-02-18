using System.Collections.Generic;
using Vagabondo.Grammar;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{
    public class IngredientDefinition : IGrammarNoun
    {
        public string name { get; set; }
        public bool isPluralizable { get; set; }

        public ItemSubcategory subcategory;
        public int frequency;
        public int baseValue;
        public int nutrition;
        public UseVerb useVerb = UseVerb.None;
        public Dictionary<Biome, float> biomes = new();
        public Dictionary<TownTrait, float> traits = new();


        public GameItem Instantiate()
        {
            var item = new GameItem();
            if (isPluralizable)
                item.name = RichGrammarModifiers.applyModifier(name, "s");
            else
                item.name = name;

            item.category = ItemCategory.FoodIngredient;
            item.subcategory = subcategory;
            item.baseValue = baseValue;
            item.currentPrice = baseValue;
            item.nutrition = nutrition;
            item.useVerb = useVerb;

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


    public class GameItemTemplate
    {
        public string name;
        public ItemSubcategory subcategory;
        public List<ItemSubcategory> ingredientCategories = new();
        public List<string> ingredientNames = new();
        public Preparation preparation;
        public int frequency = 1;
        public UseVerb useVerb;

    }
}
