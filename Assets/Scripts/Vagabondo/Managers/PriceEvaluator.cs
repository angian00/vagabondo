using System;
using System.Collections.Generic;
using Vagabondo.DataModel;

namespace Vagabondo.Managers
{
    public class PriceEvaluator
    {
        private static Dictionary<ItemQuality, float> qualityValueMultiplier = new()
        {
            { ItemQuality.Terrible, 0.2f },
            { ItemQuality.Poor, 0.6f },
            { ItemQuality.Standard, 1.0f },
            { ItemQuality.Good, 1.3f },
            { ItemQuality.Exceptional, 2.0f },
        };


        public static void UpdatePrices(List<GameItem> items)
        {
            foreach (var item in items)
                UpdatePrice(item);
        }

        public static void UpdatePrice(GameItem item)
        {
            //TODO: use townData to influence currentPrice
            item.currentPrice = (int)Math.Round(item.baseValue * qualityValueMultiplier[item.quality]);
        }
    }
}
