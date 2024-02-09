using System;
using System.Collections.Generic;
using Vagabondo.DataModel;

namespace Vagabondo.Generators
{
    public class MerchandiseGenerator
    {
        public static TradableItem GenerateItem(ItemCategory category)
        {
            switch (category)
            {
                case ItemCategory.WildPlant:
                    return generateWildPlant();

                case ItemCategory.Tool:
                    return generateItemOther("hammer", ItemCategory.Tool);

                default:
                    throw new NotImplementedException();
            }
        }

        private static TradableItem generateWildPlant()
        {
            var plant = new Plant();
            plant.name = FileStringGenerator.WildPlants.GenerateString();
            plant.quality = FoodGenerator.randomQuality();
            plant.baseValue = 10;

            return plant;
        }

        private static TradableItem generateItemOther(string name, ItemCategory category)
        {
            var item = new HouseholdItem();
            item.name = name;
            item.category = category;
            item.quality = FoodGenerator.randomQuality();
            item.baseValue = 10;

            return item;
        }

        public static List<TradableItem> GenerateInventory(ShopType shopType)
        {
            switch (shopType)
            {
                case ShopType.Tavern:
                case ShopType.Bakery:
                    return GenerateInventoryFood(shopType);

                case ShopType.Butchery:
                    return GenerateInventoryFoodIngredients(shopType);

                default:
                    throw new NotImplementedException();
            }
        }


        private static List<TradableItem> GenerateInventoryFood(ShopType shopType)
        {
            //FUTURE: inventory is influenced by town data
            const int inventorySize = 12;

            var result = new List<TradableItem>();
            Predicate<FoodItem> itemFilter;

            switch (shopType)
            {
                case ShopType.Tavern:
                    itemFilter = (foodItem) => (foodItem.foodCategory == FoodItemCategory.Drink);
                    break;

                case ShopType.Bakery:
                    //itemFilter = (foodItem) => (foodItem.category == FoodItemCategory.Bread || foodItem.category == FoodItemCategory.Dessert);
                    itemFilter = (foodItem) => (foodItem.foodCategory == FoodItemCategory.Bread);
                    break;

                case ShopType.Butchery:
                    itemFilter = (foodItem) => (foodItem.foodCategory == FoodItemCategory.Bread);
                    break;

                default:
                    throw new NotImplementedException();
            }

            result.AddRange(FoodGenerator.GenerateFoodItems(itemFilter, inventorySize));

            return result;
        }

        private static List<TradableItem> GenerateInventoryFoodIngredients(ShopType shopType)
        {
            //FUTURE: inventory is influenced by town data
            const int inventorySize = 12;

            var result = new List<TradableItem>();
            Predicate<FoodIngredientDef> ingredientFilter;

            switch (shopType)
            {
                case ShopType.Butchery:
                    ingredientFilter = (foodIngredientDef) => (foodIngredientDef.category == FoodIngredientCategory.Meat);
                    break;

                default:
                    throw new NotImplementedException();
            }

            result.AddRange(FoodGenerator.GenerateFoodIngredients(ingredientFilter, inventorySize));

            return result;
        }
    }
}