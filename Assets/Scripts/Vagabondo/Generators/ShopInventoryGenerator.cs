using System;
using System.Collections.Generic;
using Vagabondo.DataModel;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class ShopInventoryGenerator
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

        public static List<GameItem> GenerateInventory(ShopType shopType, int inventorySize)
        {
            switch (shopType)
            {
                case ShopType.Tavern:
                case ShopType.Bakery:
                    return GenerateInventoryItems(shopType, inventorySize);

                case ShopType.Butchery:
                case ShopType.Farm:
                    return GenerateInventoryIngredients(shopType, inventorySize);

                case ShopType.Library:
                    return GenerateInventoryBooks(inventorySize);

                default:
                    throw new NotImplementedException();
            }
        }

        private static List<GameItem> GenerateInventoryBooks(int inventorySize)
        {
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

        private static List<GameItem> GenerateInventoryItems(ShopType shopType, int inventorySize)
        {
            var result = new List<GameItem>();
            Predicate<GameItem> itemFilter;

            switch (shopType)
            {
                case ShopType.Tavern:
                    itemFilter = (item) => (item.subcategory == ItemSubcategory.Drink);
                    break;

                case ShopType.Bakery:
                    itemFilter = (item) => (item.subcategory == ItemSubcategory.Bread ||
                                                item.subcategory == ItemSubcategory.Dessert);
                    break;

                case ShopType.Butchery:
                    itemFilter = (item) => (item.subcategory == ItemSubcategory.Bread);
                    break;

                default:
                    throw new NotImplementedException();
            }

            result.AddRange(GameItemGenerator.GenerateItemsFromTemplates(itemFilter, inventorySize));

            return result;
        }

        private static List<GameItem> GenerateInventoryIngredients(ShopType shopType, int inventorySize)
        {
            var result = new List<GameItem>();
            Predicate<IngredientDefinition> ingredientFilter;

            switch (shopType)
            {
                case ShopType.Butchery:
                    ingredientFilter = (ingredientDef) => (ingredientDef.subcategory == ItemSubcategory.Meat);
                    break;

                case ShopType.Farm:
                    ingredientFilter = (ingredientDef) =>
                        (ingredientDef.subcategory == ItemSubcategory.Grain ||
                        ingredientDef.subcategory == ItemSubcategory.Vegetable ||
                        ingredientDef.subcategory == ItemSubcategory.Legume ||
                        ingredientDef.subcategory == ItemSubcategory.Fruit);
                    break;


                default:
                    throw new NotImplementedException();
            }

            result.AddRange(GameItemGenerator.GenerateIngredients(ingredientFilter, inventorySize));

            return result;
        }
    }
}