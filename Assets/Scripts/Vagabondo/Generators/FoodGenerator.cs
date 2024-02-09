using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Grammar;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
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


        public static List<FoodIngredient> GenerateFoodIngredients(int nIngredients = 10)
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

        public static List<FoodIngredient> GenerateFoodIngredients(List<FoodIngredientCategory> categories, int nIngredients = 10)
        {
            Predicate<FoodIngredientDef> ingredientFilter = (ingredientDef) => categories.Contains(ingredientDef.category);
            return GenerateFoodIngredients(ingredientFilter, nIngredients);
        }

        public static List<FoodIngredient> GenerateFoodIngredients(Predicate<FoodIngredientDef> ingredientFilter, int nIngredients = 10)
        {
            List<FoodIngredientDef> compatibleDefs = new();
            List<int> compatibleDefWeights = new();

            for (int iDef = 0; iDef < ingredientDefinitions.Count; iDef++)
            {
                var def = ingredientDefinitions[iDef];

                if (ingredientFilter(def))
                {
                    compatibleDefs.Add(def);
                    compatibleDefWeights.Add(ingredientDefinitionWeights[iDef]);
                }
            }

            List<FoodIngredient> res = new();
            for (int iIngredient = 0; iIngredient < nIngredients; iIngredient++)
            {
                var ingredient = new FoodIngredient();
                ingredient.definition = RandomUtils.RandomChooseWeighted(compatibleDefs, compatibleDefWeights);
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

                foodItem.foodCategory = template.category;
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
                foodItem.quality = computeFoodQuality(foodItem);
                foodItem.baseValue = computeFoodBaseValue(foodItem);
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
            //var availableIngredients = GenerateFoodIngredients(biome, nIngredients);
            var availableIngredients = GenerateFoodIngredients(nIngredients);

            Debug.Log("Generated ingredients:");
            foreach (var ingredient in availableIngredients)
                Debug.Log($"\t {ingredient.definition.name} [{DataUtils.EnumToStr(ingredient.definition.category)}]");

            return GenerateFoodItem(availableIngredients);
        }

        public static List<FoodItem> GenerateFoodItems(Predicate<FoodItem> itemFilter, int nItems = 10)
        {
            var nIngredients = nItems * 100;
            var availableIngredients = GenerateFoodIngredients(nIngredients);

            var nTries = nItems * 100;
            var iTry = 0;
            List<FoodItem> res = new();
            while (true)
            {
                iTry++;
                if (res.Count >= nItems || iTry >= nTries)
                    break;

                var item = GenerateFoodItem(availableIngredients);
                if (item != null && itemFilter == null || itemFilter(item))
                    res.Add(item);
            }

            return res;
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

        private static int computeFoodBaseValue(FoodItem foodItem)
        {
            const float preparationMultiplier = 1.2f;
            var cumValue = 0.0f;
            foreach (var ingredient in foodItem.ingredients)
            {
                cumValue += ingredient.definition.baseValue;
            }

            cumValue *= preparationMultiplier;

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
            if (foodItem.quality == ItemQuality.Standard)
                qualityStr = "";
            else
                qualityStr = " [" + DataUtils.EnumToStr(foodItem.quality).ToLower() + "]";

            return dishStr + qualityStr;
        }


        public static ItemQuality randomQuality()
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