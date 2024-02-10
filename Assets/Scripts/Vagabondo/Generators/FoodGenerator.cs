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
        private static List<ItemTemplate> foodItemTemplates;
        private static List<int> ingredientDefinitionWeights;
        private static List<int> foodItemTemplateWeights;

        static FoodGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodIngredientDefinitions");
            ingredientDefinitions = JsonConvert.DeserializeObject<List<IngredientDefinition>>(fileObj.text);
            ingredientDefinitionWeights = new();
            foreach (var definition in ingredientDefinitions)
                ingredientDefinitionWeights.Add(definition.frequency);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodItemTemplates");
            foodItemTemplates = JsonConvert.DeserializeObject<List<ItemTemplate>>(fileObj.text);
            foodItemTemplateWeights = new();
            foreach (var template in foodItemTemplates)
                foodItemTemplateWeights.Add(template.frequency);
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

        public static GameItem GenerateFoodIngredient(Predicate<IngredientDefinition> ingredientFilter)
        {
            var ingredientList = GenerateFoodIngredients(ingredientFilter, nIngredients: 1);
            return ingredientList[0];
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


        public static GameItem GenerateFoodItemFromIngredients(List<GameItem> availableIngredients)
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

        public static GameItem GenerateIngredientsAndFood(int nIngredients = 10)
        {
            var availableIngredients = GenerateFoodIngredients(nIngredients);

            Debug.Log("Generated ingredients:");
            foreach (var ingredient in availableIngredients)
                Debug.Log($"\t {ingredient.definition.name} [{DataUtils.EnumToStr(ingredient.definition.subcategory)}]");

            return GenerateFoodItemFromIngredients(availableIngredients);
        }

        public static List<GameItem> GenerateFoodItemsFromTemplates(Predicate<GameItem> itemFilter, int nItems = 10)
        {
            var nTries = nItems * 10;
            var iTry = 0;
            List<GameItem> res = new();
            while (true)
            {
                iTry++;
                if (res.Count >= nItems || iTry >= nTries)
                    break;

                var item = GenerateFoodItemFromTemplates();
                if (item != null && (itemFilter == null || itemFilter(item)))
                    res.Add(item);
            }

            return res;
        }


        public static GameItem GenerateFoodItemFromTemplates()
        {
            const int maxTries = 20;
            int nTries = 0;

            while (true)
            {
                var template = RandomUtils.RandomChooseWeighted(foodItemTemplates, foodItemTemplateWeights);
                var foodItem = new GameItem();

                var chosenIngredients = new List<GameItem>();
                var chosenIngredientNames = new List<string>();

                foreach (var ingredientName in template.ingredientNames)
                {
                    var ingredientDef = ingredientDefinitions.Find(def => def.name == ingredientName);
                    var ingredient = ingredientDef.Instantiate();

                    chosenIngredients.Add(ingredient);
                    chosenIngredientNames.Add(ingredient.definition.name);
                }

                foreach (var ingredientCategory in template.ingredientCategories)
                {
                    Predicate<IngredientDefinition> ingredientFilter = (def) => (def.subcategory == ingredientCategory);
                    GameItem ingredient;
                    while (true)
                    {
                        ingredient = GenerateFoodIngredient(ingredientFilter);
                        if (ingredient == null)
                            goto outerLoopIterate;

                        if (!chosenIngredientNames.Contains(ingredient.name))
                            //ok
                            break;
                    }

                    chosenIngredients.Add(ingredient);
                    chosenIngredientNames.Add(ingredient.definition.name);
                }

                foodItem.subcategory = template.subcategory;

                var preparationQuality = RandomUtils.RandomQuality();
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


        public static List<GameItem> GenerateFoodItems__old(Predicate<GameItem> itemFilter, int nItems = 10)
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

                var item = GenerateFoodItemFromIngredients(availableIngredients);
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


        private static string computeFoodName(List<GameItem> ingredients, ItemTemplate template)
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