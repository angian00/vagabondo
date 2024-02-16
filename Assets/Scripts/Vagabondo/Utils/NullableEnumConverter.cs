using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;


namespace Vagabondo.Utils
{
    public class NullableEnumConverter<T> : StringEnumConverter where T : struct
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsEnum && base.CanConvert(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.String && string.IsNullOrWhiteSpace(reader.Value?.ToString()))
            {
                return null;
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            base.WriteJson(writer, value, serializer);
        }
    }
}
