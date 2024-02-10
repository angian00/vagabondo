using System;
using Vagabondo.DataModel;

namespace Vagabondo.Utils
{
    public class StringUtils
    {
        private static string styleGood = "GOOD";
        private static string styleBad = "BAD";

        public static string BuildResultTextStat(StatId stat, int amount)
        {
            return buildResultText(DataUtils.EnumToStr(stat), amount);
        }

        public static string BuildResultTextHealth(int amount)
        {
            return buildResultText("Health", amount);
        }

        public static string BuildResultTextMoney(int amount)
        {
            return buildResultText("$", amount);
        }

        private static string buildResultText(string label, int amount)
        {
            string styleStr;
            string signStr;
            if (amount > 0)
            {
                styleStr = styleGood;
                signStr = "+";
            }
            else
            {
                styleStr = styleBad;
                signStr = "-";
            }

            return $"<style={styleStr}>{signStr}{Math.Abs(amount)} {label}</style>";
        }

        public static string BuildResultTextItem(GameItem item, bool isAcquiring)
        {
            string styleStr;
            string signStr;

            if (isAcquiring)
            {
                styleStr = styleGood;
                signStr = "+";
            }
            else
            {
                styleStr = styleBad;
                signStr = "-";
            }

            //var itemStr = item.isPluralizable ? item.name : item.name.Pluralize();
            var itemStr = item.name;
            if (item.quality != ItemQuality.Standard)
                itemStr += $" [{DataUtils.EnumToStr(item.quality)}]";

            return $"<style={styleStr}>{signStr} {itemStr}</style>";
        }
    }
}
