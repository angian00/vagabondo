using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vagabondo.Grammar;
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


    public class FoodItem
    {
        public string name;
        public FoodItemCategory category;
        public List<FoodIngredient> ingredients = new();
        public FoodPreparation preparation;
        public ItemQuality preparationQuality;
        public ItemQuality overallQuality;
        public int baseValue;

        //FUTURE: perishability
    }


    public class FoodGenerator
    {
        private static List<FoodIngredientDef> ingredientDefinitions;
        private static List<FoodItemTemplate> foodItemTemplates;
        private static List<int> ingredientDefinitionWeights;

        private static Dictionary<ItemQuality, float> qualityValueMultiplier = new()
        {
            { ItemQuality.Terrible, 0.2f },
            { ItemQuality.Poor, 0.6f },
            { ItemQuality.Standard, 1.0f },
            { ItemQuality.Good, 1.3f },
            { ItemQuality.Exceptional, 2.0f },
        };

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
            const int maxTries = 20;
            int nTries = 0;

            while (true)
            {
                var availableIngredientsCopy = new List<FoodIngredient>(availableIngredients);
                var template = RandomUtils.RandomChoose(foodItemTemplates);

                var foodItem = new FoodItem();

                foodItem.category = template.category;
                foodItem.preparation = template.preparation;
                foodItem.preparationQuality = randomQuality();

                var chosenIngredients = new List<FoodIngredient>();
                var chosenIngredientNames = new List<string>();

                foreach (var ingredientName in template.ingredientNames)
                {
                    var ingredient = chooseIngredient(ingredientName, availableIngredientsCopy, chosenIngredientNames);
                    if (ingredient == null)
                        goto outerLoopIterate;

                    chosenIngredients.Add(ingredient);
                    chosenIngredientNames.Add(ingredient.definition.name);
                    availableIngredientsCopy.Remove(ingredient);
                }

                foreach (var ingredientCategory in template.ingredientCategories)
                {
                    var ingredient = chooseIngredient(ingredientCategory, availableIngredientsCopy, chosenIngredientNames);
                    if (ingredient == null)
                        goto outerLoopIterate;

                    chosenIngredients.Add(ingredient);
                    chosenIngredientNames.Add(ingredient.definition.name);
                    availableIngredientsCopy.Remove(ingredient);
                }

                foodItem.ingredients = chosenIngredients;
                foodItem.overallQuality = computeFoodQuality(foodItem);
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

        private static FoodIngredient chooseIngredient(FoodIngredientCategory targetCategory,
            List<FoodIngredient> availableIngredients, List<string> prohibitedIngredientNames)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient =>
                (ingredient.definition.category == targetCategory &&
                !prohibitedIngredientNames.Contains(ingredient.definition.name))).ToList();

            if (compatibleIngredients.Count == 0)
                return null;

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static FoodIngredient chooseIngredient(string targetIngredientName,
            List<FoodIngredient> availableIngredients, List<string> prohibitedIngredientNames)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient =>
                (ingredient.definition.name == targetIngredientName &&
                !prohibitedIngredientNames.Contains(ingredient.definition.name))).ToList();

            if (compatibleIngredients.Count == 0)
                return null;

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static ItemQuality computeFoodQuality(FoodItem foodItem)
        {
            const float preparationWeight = 2.0f;
            float cumQuality = 0;
            float nQualities = 0;

            foreach (var ingredient in foodItem.ingredients)
            {
                cumQuality += (int)ingredient.quality;
                nQualities++;
            }

            cumQuality += preparationWeight * (int)foodItem.preparationQuality;
            nQualities += preparationWeight;


            return (ItemQuality)Math.Round(cumQuality / nQualities);
        }

        private static int computeFoodValue(FoodItem foodItem)
        {
            const float preparationMultiplier = 1.2f;
            var cumValue = 0.0f;
            foreach (var ingredient in foodItem.ingredients)
            {
                cumValue += ingredient.definition.baseValue;
            }

            cumValue *= preparationMultiplier * qualityValueMultiplier[foodItem.overallQuality];

            return (int)Math.Round(cumValue);
        }


        private static string computeFoodName(FoodItem foodItem, FoodItemTemplate template)
        {
            var ingredientNouns = new List<IGrammarNoun>();
            for (var iIngredient = 0; iIngredient < foodItem.ingredients.Count; iIngredient++)
            {
                var ingredient = foodItem.ingredients[iIngredient];
                ingredientNouns.Add(ingredient.definition);
            }

            var dishStr = RefExpansion.ExpandText(template.name, "ingredient", ingredientNouns);

            string qualityStr;
            if (foodItem.overallQuality == ItemQuality.Standard)
                qualityStr = "";
            else
                qualityStr = " [" + DataUtils.EnumToStr(foodItem.overallQuality).ToLower() + "]";

            return dishStr + qualityStr;
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
            var values = new List<ItemQuality>() {
                ItemQuality.Terrible,
                ItemQuality.Poor,
                ItemQuality.Standard,
                ItemQuality.Good,
                ItemQuality.Exceptional
            };
            var weights = new List<int>() { 1, 10, 60, 10, 1 };

            return RandomUtils.RandomChooseWeighted(values, weights);
        }
    }
}