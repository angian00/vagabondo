using System;
using Vagabondo.Utils;

namespace Vagabondo.Generators
{
    public class EnumGenerator
    {
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