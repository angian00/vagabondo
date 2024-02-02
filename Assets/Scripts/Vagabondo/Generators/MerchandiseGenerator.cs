using UnityEngine;
using Vagabondo.DataModel;
using static Vagabondo.Grammar.GrammarFactory;

namespace Vagabondo.Generators
{
    public class MerchandiseGenerator
    {
        public static MerchandiseItem GenerateHerb(Biome biome)
        {
            var merchItem = new MerchandiseItem();
            merchItem.category = MerchandiseItem.Category.Herb;
            merchItem.text = FileStringGenerator.Herbs.GenerateString();
            merchItem.basePrice = Random.Range(1, 200);

            return merchItem;
        }

        public static MerchandiseItem GenerateFood(Town town)
        {
            var merchItem = new MerchandiseItem();
            merchItem.category = MerchandiseItem.Category.Food;
            merchItem.text = GetGrammar(GrammarId.Food).GenerateText();
            merchItem.basePrice = Random.Range(1, 200);
            merchItem.quality = MerchandiseItem.Quality.Standard;

            return merchItem;
        }
    }
}