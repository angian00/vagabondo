using System;
using System.Collections.Generic;
using System.Linq;


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
            return Enum.GetName(typeof(T), value);
        }

        public static T StrToEnum<T>(string valueStr) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), valueStr, true);
        }

        public static IEnumerable<T> EnumValues<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }

    }

}
