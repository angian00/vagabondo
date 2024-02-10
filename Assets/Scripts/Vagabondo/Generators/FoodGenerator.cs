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
        private static List<IngredientDefinition> ingredientDefinitions;
        private static List<FoodItemTemplate> foodItemTemplates;
        private static List<int> ingredientDefinitionWeights;

        static FoodGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodIngredientDefinitions");
            ingredientDefinitions = JsonConvert.DeserializeObject<List<IngredientDefinition>>(fileObj.text);
            ingredientDefinitionWeights = new();
            foreach (var id in ingredientDefinitions)
                ingredientDefinitionWeights.Add(id.frequency);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodItemTemplates");
            foodItemTemplates = JsonConvert.DeserializeObject<List<FoodItemTemplate>>(fileObj.text);
        }


        public static List<GameItem> GenerateFoodIngredients(int nIngredients = 10)
        {
            List<GameItem> res = new();
            for (int iIngredient = 0; iIngredient < nIngredients; iIngredient++)
            {
                var ingredient = new GameItem();
                ingredient.definition = RandomUtils.RandomChooseWeighted(ingredientDefinitions, ingredientDefinitionWeights);
                ingredient.quality = RandomUtils.RandomQuality();
                res.Add(ingredient);
            }

            return res;
        }

        public static List<GameItem> GenerateFoodIngredients(List<ItemSubcategory> categories, int nIngredients = 10)
        {
            Predicate<IngredientDefinition> ingredientFilter = (ingredientDef) => categories.Contains(ingredientDef.subcategory);
            return GenerateFoodIngredients(ingredientFilter, nIngredients);
        }

        public static List<GameItem> GenerateFoodIngredients(Predicate<IngredientDefinition> ingredientFilter, int nIngredients = 10)
        {
            List<IngredientDefinition> compatibleDefs = new();
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

            List<GameItem> res = new();
            for (int iIngredient = 0; iIngredient < nIngredients; iIngredient++)
            {
                var ingredientDef = RandomUtils.RandomChooseWeighted(compatibleDefs, compatibleDefWeights);
                res.Add(ingredientDef.Instantiate());
            }

            return res;
        }


        public static GameItem GenerateFoodItem(List<GameItem> availableIngredients)
        {
            const int maxTries = 20;
            int nTries = 0;

            while (true)
            {
                var availableIngredientsCopy = new List<GameItem>(availableIngredients);
                var template = RandomUtils.RandomChoose(foodItemTemplates);

                var foodItem = new GameItem();

                //template.preparation;
                var preparationQuality = RandomUtils.RandomQuality();

                var chosenIngredients = new List<GameItem>();
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

                foodItem.subcategory = template.subcategory;
                foodItem.quality = computeFoodQuality(chosenIngredients, preparationQuality);
                foodItem.baseValue = computeFoodBaseValue(chosenIngredients);
                foodItem.name = computeFoodName(chosenIngredients, template);

                return foodItem;

            outerLoopIterate:
                nTries++;
                if (nTries >= maxTries)
                    return null;
            }
        }

        public static GameItem GenerateFoodItem(int nIngredients = 10)
        {
            var availableIngredients = GenerateFoodIngredients(nIngredients);

            Debug.Log("Generated ingredients:");
            foreach (var ingredient in availableIngredients)
                Debug.Log($"\t {ingredient.definition.name} [{DataUtils.EnumToStr(ingredient.definition.subcategory)}]");

            return GenerateFoodItem(availableIngredients);
        }

        public static List<GameItem> GenerateFoodItems(Predicate<GameItem> itemFilter, int nItems = 10)
        {
            var nIngredients = nItems * 100;
            var availableIngredients = GenerateFoodIngredients(nIngredients);

            var nTries = nItems * 100;
            var iTry = 0;
            List<GameItem> res = new();
            while (true)
            {
                iTry++;
                if (res.Count >= nItems || iTry >= nTries)
                    break;

                var item = GenerateFoodItem(availableIngredients);
                if (item != null && (itemFilter == null || itemFilter(item)))
                    res.Add(item);
            }

            return res;
        }


        private static GameItem chooseIngredient(ItemSubcategory targetCategory,
            List<GameItem> availableIngredients, List<string> prohibitedIngredientNames)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient =>
                (ingredient.definition.subcategory == targetCategory &&
                !prohibitedIngredientNames.Contains(ingredient.definition.name))).ToList();

            if (compatibleIngredients.Count == 0)
                return null;

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static GameItem chooseIngredient(string targetIngredientName,
            List<GameItem> availableIngredients, List<string> prohibitedIngredientNames)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient =>
                (ingredient.definition.name == targetIngredientName &&
                !prohibitedIngredientNames.Contains(ingredient.definition.name))).ToList();

            if (compatibleIngredients.Count == 0)
                return null;

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static ItemQuality computeFoodQuality(List<GameItem> ingredients, ItemQuality preparationQuality)
        {
            const float preparationWeight = 2.0f;
            float cumQuality = 0;
            float nQualities = 0;

            foreach (var ingredient in ingredients)
            {
                cumQuality += (int)ingredient.quality;
                nQualities++;
            }

            cumQuality += preparationWeight * (int)preparationQuality;
            nQualities += preparationWeight;


            return (ItemQuality)Math.Round(cumQuality / nQualities);
        }

        private static int computeFoodBaseValue(List<GameItem> ingredients)
        {
            const float preparationMultiplier = 1.2f;
            var cumValue = 0.0f;
            foreach (var ingredient in ingredients)
            {
                cumValue += ingredient.definition.baseValue;
            }

            cumValue *= preparationMultiplier;

            return (int)Math.Round(cumValue);
        }


        private static string computeFoodName(List<GameItem> ingredients, FoodItemTemplate template)
        {
            var ingredientNouns = new List<IGrammarNoun>();
            for (var iIngredient = 0; iIngredient < ingredients.Count; iIngredient++)
            {
                var ingredient = ingredients[iIngredient];
                ingredientNouns.Add(ingredient.definition);
            }

            return RefExpansion.ExpandText(template.name, "ingredient", ingredientNouns);
            //string qualityStr;
            //if (foodItem.quality == ItemQuality.Standard)
            //    qualityStr = "";
            //else
            //    qualityStr = " [" + DataUtils.EnumToStr(foodItem.quality).ToLower() + "]";

            //return dishStr + qualityStr;
        }

    }
}