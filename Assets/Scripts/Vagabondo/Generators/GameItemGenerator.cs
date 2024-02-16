using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vagabondo.DataModel;
using Vagabondo.Grammar;
using Vagabondo.Managers;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class GameItemGenerator
    {
        private static List<IngredientDefinition> ingredientDefinitions;
        private static List<GameItemTemplate> templates;
        private static List<int> ingredientDefinitionWeights;
        private static List<int> templateWeights;

        static GameItemGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodIngredientDefinitions");
            ingredientDefinitions = JsonConvert.DeserializeObject<List<IngredientDefinition>>(fileObj.text);
            ingredientDefinitionWeights = new();
            foreach (var definition in ingredientDefinitions)
                ingredientDefinitionWeights.Add(definition.frequency);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodItemTemplates");
            templates = JsonConvert.DeserializeObject<List<GameItemTemplate>>(fileObj.text);
            templateWeights = new();
            foreach (var template in templates)
                templateWeights.Add(template.frequency);
        }


        public static List<GameItem> GenerateIngredients(int nIngredients = 10)
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

        public static List<GameItem> GenerateIngredients(List<ItemSubcategory> categories, int nIngredients = 10)
        {
            Predicate<IngredientDefinition> ingredientFilter = (ingredientDef) => categories.Contains(ingredientDef.subcategory);
            return GenerateIngredients(ingredientFilter, nIngredients);
        }

        public static GameItem GenerateIngredient(Predicate<IngredientDefinition> ingredientFilter)
        {
            var ingredientList = GenerateIngredients(ingredientFilter, nIngredients: 1);
            return ingredientList[0];
        }

        public static List<GameItem> GenerateIngredients(Predicate<IngredientDefinition> ingredientFilter, int nIngredients = 10)
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


        public static GameItem GenerateItemFromIngredients(List<GameItem> availableIngredients)
        {
            const int maxTries = 20;
            int nTries = 0;

            while (true)
            {
                var availableIngredientsCopy = new List<GameItem>(availableIngredients);
                var template = RandomUtils.RandomChoose(templates);

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

                return generateItem(template, chosenIngredients);

            outerLoopIterate:
                nTries++;
                if (nTries >= maxTries)
                    return null;
            }
        }

        public static GameItem GenerateIngredientsAndItem(int nIngredients = 10)
        {
            var availableIngredients = GenerateIngredients(nIngredients);

            Debug.Log("Generated ingredients:");
            foreach (var ingredient in availableIngredients)
                Debug.Log($"\t {ingredient.definition.name} [{DataUtils.EnumToStr(ingredient.definition.subcategory)}]");

            return GenerateItemFromIngredients(availableIngredients);
        }

        public static List<GameItem> GenerateItemsFromTemplates(Predicate<GameItem> itemFilter, int nItems = 10)
        {
            var nTries = nItems * 10;
            var iTry = 0;
            List<GameItem> res = new();
            while (true)
            {
                iTry++;
                if (res.Count >= nItems || iTry >= nTries)
                    break;

                var item = GenerateItemFromTemplates();
                if (item != null && (itemFilter == null || itemFilter(item)))
                    res.Add(item);
            }

            return res;
        }


        public static GameItem GenerateItemFromTemplates()
        {
            const int maxTries = 20;
            int nTries = 0;

            while (true)
            {
                var template = RandomUtils.RandomChooseWeighted(templates, templateWeights);

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
                        ingredient = GenerateIngredient(ingredientFilter);
                        if (ingredient == null)
                            goto outerLoopIterate;

                        if (!chosenIngredientNames.Contains(ingredient.name))
                            //ok
                            break;
                    }

                    chosenIngredients.Add(ingredient);
                    chosenIngredientNames.Add(ingredient.definition.name);
                }


                return generateItem(template, chosenIngredients);

            outerLoopIterate:
                nTries++;
                if (nTries >= maxTries)
                    return null;
            }
        }

        private static GameItem generateItem(GameItemTemplate template, List<GameItem> ingredients)
        {
            var item = new GameItem();

            item.name = computeItemName(ingredients, template);
            item.category = ItemCategory.Food;
            item.subcategory = template.subcategory;
            item.useVerb = (template.useVerb == UseVerb.None ? UseVerb.Eat : template.useVerb);

            var preparationQuality = RandomUtils.RandomQuality();
            item.quality = computeItemQuality(ingredients, preparationQuality);

            item.baseValue = computeItemBaseValue(ingredients);
            item.nutrition = computeItemNutrition(ingredients);

            return item;
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

        private static ItemQuality computeItemQuality(List<GameItem> ingredients, ItemQuality preparationQuality)
        {
            float cumQuality = 0;
            float nQualities = 0;

            foreach (var ingredient in ingredients)
            {
                cumQuality += (int)ingredient.quality;
                nQualities++;
            }

            cumQuality += GameParams.Instance.preparationQualityWeight * (int)preparationQuality;
            nQualities += GameParams.Instance.preparationQualityWeight;


            return (ItemQuality)Math.Round(cumQuality / nQualities);
        }

        private static int computeItemBaseValue(List<GameItem> ingredients)
        {

            var cumValue = 0.0f;
            foreach (var ingredient in ingredients)
            {
                cumValue += ingredient.definition.baseValue;
            }

            cumValue *= GameParams.Instance.preparationValueMultiplier;

            return (int)Math.Round(cumValue);
        }

        private static int computeItemNutrition(List<GameItem> ingredients)
        {

            var cumNutrition = 0.0f;
            foreach (var ingredient in ingredients)
            {
                cumNutrition += ingredient.definition.nutrition;
            }

            //cumNutrition *= GameParams.Instance.preparationValueMultiplier;

            return (int)Math.Round(cumNutrition);
        }

        private static string computeItemName(List<GameItem> ingredients, GameItemTemplate template)
        {
            var ingredientNouns = new List<IGrammarNoun>();
            for (var iIngredient = 0; iIngredient < ingredients.Count; iIngredient++)
            {
                var ingredient = ingredients[iIngredient];
                ingredientNouns.Add(ingredient.definition);
            }

            return RefExpansion.ExpandText(template.name, "ingredient", ingredientNouns);
        }

    }
}