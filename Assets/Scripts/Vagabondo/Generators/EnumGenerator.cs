using System;
using Vagabondo.DataModel;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class EnumGenerator
    {
        public static EnumGeneratorGeneric<TownSize> TownSize = new EnumGeneratorGeneric<TownSize>("townSizes");

        public class EnumGeneratorGeneric<T> : FileStringGenerator where T : Enum
        {
            public EnumGeneratorGeneric(string sourceFile) : base(sourceFile) { }

            public T GenerateValue()
            {
                var valueStr = GenerateString();
                return DataUtils.StrToEnum<T>(valueStr);
            }
        }
    }

}