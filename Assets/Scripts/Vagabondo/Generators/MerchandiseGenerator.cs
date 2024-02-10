using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class MerchandiseGenerator
    {
        public static GameItem GenerateItem(ItemCategory category)
        {
            switch (category)
            {
                case ItemCategory.WildPlant:
                    return generateItem(FileStringGenerator.WildPlants.GenerateString(), ItemCategory.WildPlant);

                case ItemCategory.Tool:
                    return generateItem("hammer", ItemCategory.Tool);

                default:
                    throw new NotImplementedException();
            }
        }

        private static GameItem generateItem(string name, ItemCategory category)
        {
            var item = new GameItem();
            item.name = name;
            item.category = category;
            item.quality = RandomUtils.RandomQuality();
            item.baseValue = 10;

            return item;
        }

        public static List<GameItem> GenerateInventory(ShopType shopType)
        {
            switch (shopType)
            {
                case ShopType.Tavern:
                case ShopType.Bakery:
                    return GenerateInventoryFood(shopType);

                case ShopType.Butchery:
                    return GenerateInventoryFoodIngredients(shopType);

                case ShopType.Library:
                    return GenerateInventoryBooks();

                default:
                    throw new NotImplementedException();
            }
        }

        private static List<GameItem> GenerateInventoryBooks()
        {
            //FUTURE: inventory is influenced by town data
            const int inventorySize = 12;

            var result = new List<GameItem>();
            for (int i = 0; i < inventorySize; i++)
            {
                var book = new GameItem();
                book.name = "Some book title";
                book.category = ItemCategory.Book;
                book.quality = RandomUtils.RandomQuality();
                book.baseValue = 10;
                result.Add(book);
            }

            return result;
        }

        private static List<GameItem> GenerateInventoryFood(ShopType shopType)
        {
            //FUTURE: inventory is influenced by town data
            const int inventorySize = 12;

            var result = new List<GameItem>();
            Predicate<GameItem> itemFilter;

            switch (shopType)
            {
                case ShopType.Tavern:
                    itemFilter = (foodItem) => (foodItem.subcategory == ItemSubcategory.Drink);
                    break;

                case ShopType.Bakery:
                    itemFilter = (foodItem) => (foodItem.subcategory == ItemSubcategory.Bread || foodItem.subcategory == ItemSubcategory.Dessert);
                    //itemFilter = (foodItem) => (foodItem.subcategory == ItemSubcategory.Bread);
                    break;

                case ShopType.Butchery:
                    itemFilter = (foodItem) => (foodItem.subcategory == ItemSubcategory.Bread);
                    break;

                default:
                    throw new NotImplementedException();
            }

            result.AddRange(FoodGenerator.GenerateFoodItemsFromTemplates(itemFilter, inventorySize));

            return result;
        }

        private static List<GameItem> GenerateInventoryFoodIngredients(ShopType shopType)
        {
            //FUTURE: inventory is influenced by town data
            const int inventorySize = 12;

            var result = new List<GameItem>();
            Predicate<IngredientDefinition> ingredientFilter;

            switch (shopType)
            {
                case ShopType.Butchery:
                    ingredientFilter = (foodIngredientDef) => (foodIngredientDef.subcategory == ItemSubcategory.Meat);
                    break;

                default:
                    throw new NotImplementedException();
            }

            result.AddRange(FoodGenerator.GenerateFoodIngredients(ingredientFilter, inventorySize));

            return result;
        }
    }
}