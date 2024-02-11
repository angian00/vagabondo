using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Vagabondo.Utils
{
    public class DataUtils
    {
        public static List<T> EnumToList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static string EnumToStr<T>(T value) where T : Enum
        {
            var rawStr = Enum.GetName(typeof(T), value);
            return Regex.Replace(rawStr, "([A-Z])", " $1").Trim();
        }

        public static T StrToEnum<T>(string valueStr) where T : Enum
        {
            var enumStr = valueStr.Replace(" ", "");
            return (T)Enum.Parse(typeof(T), enumStr, true);
        }

        public static IEnumerable<T> EnumValues<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }

    }

}
