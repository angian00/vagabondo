using System;
using System.Collections.Generic;

namespace Vagabondo.DataModel
{
    public enum ShopType
    {
        Bakery,
        Butchery,
        Farm,

        Tavern,
        Library,
    }

    public class ShopInfo
    {
        public string name;
        public List<GameItem> inventory;
        public Predicate<GameItem> canBuy;

        public static Dictionary<ShopType, Predicate<GameItem>> BuyFilter = new()
        {
            { ShopType.Bakery, item => (item.category == ItemCategory.FoodIngredient &&
                (item.subcategory == ItemSubcategory.Grain))},
            { ShopType.Butchery, item => (item.category == ItemCategory.FoodIngredient &&
                (item.subcategory == ItemSubcategory.Meat))},

            { ShopType.Tavern, item => (item.category == ItemCategory.Drink ||
                (item.subcategory == ItemSubcategory.Drink))},

            { ShopType.Library, item => (item.category == ItemCategory.Book)},
            { ShopType.Farm, item => false },
        };

        //public static Dictionary<ShopType, Predicate<GameItem>> SellFilter = new()
        //{
        //};

        public ShopInfo(string name, List<GameItem> inventory, Predicate<GameItem> canBuy = null)
        {
            this.name = name;
            this.inventory = inventory;
            this.canBuy = canBuy;
        }
    }

}