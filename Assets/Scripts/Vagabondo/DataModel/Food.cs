using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vagabondo.Grammar;
using Vagabondo.Utils;

namespace Vagabondo.DataModel
{
    public enum ItemQuality
    {
        Poor,
        Standard,
        High,
        Exceptional,
    }

    public enum FoodIngredientCategory
    {
        Grain,
        Vegetable,
        Legume,
        Fruit,
        Dairy,
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

    public class FoodIngredient
    {
        public FoodIngredientDef definition;
        public ItemQuality quality;
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
    }


    public enum FoodItemCategory
    {
        Raw,
        Breakfast,
        Bread,
        Soup,
        Salad,
        MainCourse,
        Dessert,
    }

    public class FoodItemTemplate
    {
        public string name;
        public FoodItemCategory category;
        public List<FoodIngredientCategory> ingredientCategories = new();
        public List<string> ingredientNames = new();
        public FoodPreparation preparation;
    }


    public class FoodItem
    {
        public string name;
        public FoodItemCategory category;
        public List<FoodIngredient> ingredients = new();
        public FoodPreparation preparation;
        public ItemQuality preparationQuality;
        public int baseValue;
        //FUTURE: perishability
    }


    public class FoodGenerator
    {
        private static List<FoodIngredientDef> ingredientDefinitions;
        private static List<FoodItemTemplate> foodItemTemplates;
        private static List<int> ingredientDefinitionWeights;

        static FoodGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodIngredientDefinitions");
            ingredientDefinitions = JsonConvert.DeserializeObject<List<FoodIngredientDef>>(fileObj.text);
            ingredientDefinitionWeights = new();
            foreach (var id in ingredientDefinitions)
                ingredientDefinitionWeights.Add(id.frequency);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodItemTemplates");
            foodItemTemplates = JsonConvert.DeserializeObject<List<FoodItemTemplate>>(fileObj.text);
        }


        public static List<FoodIngredient> GenerateFoodIngredients(Biome biome, int nIngredients = 10)
        {
            List<FoodIngredient> res = new();
            for (int iIngredient = 0; iIngredient < nIngredients; iIngredient++)
            {
                var ingredient = new FoodIngredient();
                ingredient.definition = RandomUtils.RandomChooseWeighted(ingredientDefinitions, ingredientDefinitionWeights);
                ingredient.quality = randomQuality();
                res.Add(ingredient);
            }

            return res;
        }

        public static FoodItem GenerateFoodItem(List<FoodIngredient> availableIngredients)
        {
            const int maxTries = 10;
            int nTries = 0;

            while (true)
            {
                var availableIngredientsCopy = new List<FoodIngredient>(availableIngredients);
                var template = RandomUtils.RandomChoose(foodItemTemplates);

                var foodItem = new FoodItem();

                foodItem.category = template.category;
                foodItem.preparation = template.preparation;
                foodItem.preparationQuality = randomQuality();

                foreach (var ingredientName in template.ingredientNames)
                {
                    var ingredient = chooseIngredient(availableIngredientsCopy, ingredientName);
                    if (ingredient == null)
                        goto outerLoopIterate;

                    foodItem.ingredients.Add(ingredient);
                    availableIngredientsCopy.Remove(ingredient);
                }

                foreach (var ingredientCategory in template.ingredientCategories)
                {
                    var ingredient = chooseIngredient(availableIngredientsCopy, ingredientCategory);
                    if (ingredient == null)
                        goto outerLoopIterate;

                    foodItem.ingredients.Add(ingredient);
                    availableIngredientsCopy.Remove(ingredient);
                }

                foodItem.baseValue = computeFoodValue(foodItem);
                foodItem.name = computeFoodName(foodItem, template);

                return foodItem;

            outerLoopIterate:
                nTries++;
                if (nTries >= maxTries)
                    return null;
            }
        }

        public static FoodItem GenerateFoodItem(Biome biome, int nIngredients = 10)
        {
            var availableIngredients = GenerateFoodIngredients(biome, nIngredients);

            Debug.Log("Generated ingredients:");
            foreach (var ingredient in availableIngredients)
                Debug.Log($"\t {ingredient.definition.name} [{DataUtils.EnumToStr(ingredient.definition.category)}]");

            return GenerateFoodItem(availableIngredients);
        }

        private static FoodIngredient chooseIngredient(List<FoodIngredient> availableIngredients, FoodIngredientCategory category)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient => (ingredient.definition.category == category)).ToList();
            if (compatibleIngredients.Count == 0)
                return null;

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static FoodIngredient chooseIngredient(List<FoodIngredient> availableIngredients, string ingredientName)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient => (ingredient.definition.name == ingredientName)).ToList();
            if (compatibleIngredients.Count == 0)
                return null;

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static int computeFoodValue(FoodItem foodItem)
        {
            return 10; //TODO: computeFoodValue
        }

        private static string computeFoodName(FoodItem foodItem, FoodItemTemplate template)
        {
            var ingredientNouns = new List<IGrammarNoun>();
            for (var iIngredient = 0; iIngredient < foodItem.ingredients.Count; iIngredient++)
            {
                var ingredient = foodItem.ingredients[iIngredient];
                ingredientNouns.Add(ingredient.definition);
            }

            return RefExpansion.ExpandText(template.name, "ingredient", ingredientNouns);
        }

        private static string computeFoodNameOld(FoodItem foodItem, FoodItemTemplate template)
        {
            var res = "";
            for (var iIngredient = 0; iIngredient < foodItem.ingredients.Count; iIngredient++)
            {
                var ingredient = foodItem.ingredients[iIngredient];
                if (iIngredient > 0)
                {
                    if (iIngredient == foodItem.ingredients.Count - 1)
                        res += $" and ";
                    else
                        res += $", ";
                }

                res += $"{ingredient.definition.name}";
            }

            res += $" {template.name}";

            return res;
        }

        private static ItemQuality randomQuality()
        {
            var values = new List<ItemQuality>() { ItemQuality.Poor, ItemQuality.Standard, ItemQuality.High, ItemQuality.Exceptional };
            var weights = new List<int>() { 10, 60, 10, 1 };

            return RandomUtils.RandomChooseWeighted(values, weights);
        }
    }
}