using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        //Beverage?
    }

    public struct FoodIngredientDef
    {
        public string name;
        public FoodIngredientCategory category;
        public int baseValue;
        public HashSet<Biome> compatibleBiomes;
        //TODO: "elemento" (caldo/freddo, ...)
        //
    }

    public struct FoodIngredient
    {
        public FoodIngredientDef definition;
        public ItemQuality quality;
    }

    public enum FoodPreparation
    {
        None,
        Mix,
        Boil,
        //TODO: brainstorm food preparations enum
    }


    public enum FoodItemCategory
    {
        Raw,
        Salad,
        Breakfast,
        Soup,
        Dessert,
        //TODO: brainstorm FoodItemCategory enum
    }

    public struct FoodItemTemplate
    {
        public string name;
        public FoodItemCategory category;
        public List<FoodIngredientCategory> ingredients;
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
        //FUTURE: recipe
        //FUTURE: perishability
        //FUTURE: "elemento" (caldo/freddo, ...)
    }


    public class FoodGenerator
    {
        private static List<FoodIngredientDef> ingredientDefinitions;
        private static List<FoodItemTemplate> foodItemTemplates;

        static FoodGenerator()
        {
            TextAsset fileObj;

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodIngredientDefinitions");
            ingredientDefinitions = JsonConvert.DeserializeObject<List<FoodIngredientDef>>(fileObj.text);

            fileObj = Resources.Load<TextAsset>($"Data/Generators/foodItemTemplates");
            foodItemTemplates = JsonConvert.DeserializeObject<List<FoodItemTemplate>>(fileObj.text);
        }


        public static List<FoodIngredient> GenerateFoodIngredients(Biome biome, int nIngredients = 10)
        {
            List<FoodIngredient> res = new();
            for (int iIngredient = 0; iIngredient < nIngredients; iIngredient++)
            {
                var ingredient = new FoodIngredient();
                ingredient.definition = RandomUtils.RandomChoose(ingredientDefinitions);
                ingredient.quality = ItemQuality.Standard; //TODO: randomize ingredient quality
                res.Add(ingredient);
            }

            return res;
        }

        public static FoodItem GenerateFoodItem(List<FoodIngredient> availableIngredients)
        {
            while (true)
            {
                var template = RandomUtils.RandomChoose(foodItemTemplates);

                var foodItem = new FoodItem();

                foodItem.category = template.category;
                foodItem.preparation = template.preparation;
                foodItem.preparationQuality = ItemQuality.Standard; //TODO: randomize preparationQuality

                foreach (var ingredientCategory in template.ingredients)
                {
                    //TODO: remove ingredient from availableIngredients
                    var ingredient = chooseIngredient(availableIngredients, ingredientCategory);
                    foodItem.ingredients.Add(ingredient);
                }

                foodItem.baseValue = computeFoodValue(foodItem);
                foodItem.name = computeFoodName(foodItem, template);

                //FUTURE: final foodItem compatibility check
                return foodItem;
            }
        }

        public static FoodItem GenerateFoodItem(Biome biome, int nIngredients = 10)
        {
            var availableIngredients = GenerateFoodIngredients(biome, nIngredients);
            return GenerateFoodItem(availableIngredients);
        }

        private static FoodIngredient chooseIngredient(List<FoodIngredient> availableIngredients, FoodIngredientCategory category)
        {
            var compatibleIngredients = availableIngredients.Where(ingredient => (ingredient.definition.category == category)).ToList();
            if (compatibleIngredients.Count == 0)
                throw new System.Exception("No compatible ingredients found");

            return RandomUtils.RandomChoose(compatibleIngredients);
        }

        private static int computeFoodValue(FoodItem foodItem)
        {
            return 10; //TODO: computeFoodValue
        }

        private static string computeFoodName(FoodItem foodItem, FoodItemTemplate template)
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
    }
}