using UnityEngine;
using Vagabondo.DataModel;

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

    }
}