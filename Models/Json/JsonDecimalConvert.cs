using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Newtonsoft.Json
{
    public class JsonDecimalConvert : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var strValue = ((decimal)value).ToString("#0.##");
            if (strValue.Contains('.'))
            {
                writer.WriteValue(decimal.Parse(strValue));
            }
            else
            {
                writer.WriteValue(long.Parse(strValue));
            }
        }

    }
}