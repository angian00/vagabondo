using System;
using System.Collections.Generic;
using Vagabondo.DataModel;

namespace Vagabondo.Generators
{
    public class MerchandiseGenerator
    {
        //public static MerchandiseItem GenerateHerb(Biome biome)
        //{
        //    var merchItem = new MerchandiseItem();
        //    merchItem.category = MerchandiseItem.Category.Herb;
        //    merchItem.text = FileStringGenerator.Herbs.GenerateString();
        //    merchItem.basePrice = Random.Range(1, 200);

        //    return merchItem;
        //}

        //public static MerchandiseItem GenerateFood(Town town)
        //{
        //    var merchItem = new MerchandiseItem();
        //    merchItem.category = MerchandiseItem.Category.Food;
        //    merchItem.text = GetGrammar(GrammarId.Food).GenerateText();
        //    merchItem.basePrice = Random.Range(1, 200);
        //    merchItem.quality = MerchandiseItem.Quality.Standard;

        //    return merchItem;
        //}

        public static List<TradableItem> GenerateInventoryFood(ShopType shopType)
        {
            //FUTURE: inventory is influenced by town data
            const int inventorySize = 12;

            var result = new List<TradableItem>();
            Predicate<FoodItem> itemFilter;

            switch (shopType)
            {
                case ShopType.Tavern:
                    itemFilter = (foodItem) => (foodItem.category == FoodItemCategory.Drink);
                    break;

                case ShopType.Bakery:
                    itemFilter = (foodItem) => (foodItem.category == FoodItemCategory.Bread || foodItem.category == FoodItemCategory.Dessert);
                    break;

                default:
                    throw new NotImplementedException();
            }

            result.AddRange(FoodGenerator.GenerateFoodItems(itemFilter, inventorySize));

            return result;
        }
    }
}