using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
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


        public static void UpdatePrices(List<GameItem> items, Town townData)
        {

            foreach (var item in items)
                UpdatePrice(item, townData);

            if (items.Count > 0)
            {
                StringBuilder sb = new();
                sb.Append($"Updating prices in {townData.name}");
                sb.Append("\n");
                sb.Append(townData.Dump());
                sb.Append("\n");
                sb.Append("Base values:\n");
                sb.Append(dumpBaseValues(items));
                sb.Append("\n");
                sb.Append("Current prices:\n");
                sb.Append(dumpCurrentPrices(items));
                Debug.Log(sb.ToString());
            }
        }

        public static void UpdatePrice(GameItem item, Town townData)
        {
            var totalMultiplier = 1.0f;
            totalMultiplier *= qualityValueMultiplier[item.quality];

            var abundance = 1.0f;
            abundance *= townData.baseAbundance;

            if (item.definition != null)
            {
                foreach (var biome in item.definition.biomes.Keys)
                {
                    if (townData.biome == biome)
                        abundance *= item.definition.biomes[biome];
                }

                foreach (var trait in item.definition.traits.Keys)
                {
                    if (townData.traits.Contains(trait))
                        abundance *= item.definition.traits[trait];
                }
            }


            var abundanceMultiplier = 1 / Math.Max(abundance, 0.1f);
            totalMultiplier *= abundanceMultiplier;

            if (townData.traits.Contains(TownTrait.Rich))
                totalMultiplier *= GameParams.Instance.richPriceMultiplier;
            else if (townData.traits.Contains(TownTrait.Poor))
                totalMultiplier *= GameParams.Instance.poorPriceMultiplier;

            totalMultiplier = Math.Min(totalMultiplier, GameParams.Instance.maxPriceMultiplier);
            totalMultiplier = Math.Max(totalMultiplier, GameParams.Instance.minPriceMultiplier);

            item.currentPrice = (int)Math.Round(totalMultiplier * item.baseValue);
        }

        private static string dumpBaseValues(List<GameItem> items)
        {
            string res = "";
            foreach (var item in items)
                res += item.name + " " + item.baseValue + "\n";
            return res;
        }

        private static string dumpCurrentPrices(List<GameItem> items)
        {
            string res = "";
            foreach (var item in items)
                res += item.name + " " + item.currentPrice + "\n";
            return res;
        }
    }
}
