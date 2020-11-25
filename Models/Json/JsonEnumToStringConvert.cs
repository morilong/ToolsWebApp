using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Haooyou.Tool.WebApp.Models;

namespace Newtonsoft.Json
{
    /// <summary>
    /// 将指定 T (Enum) 类型 ToString()
    /// </summary>
    /// <typeparam name="T">Enum</typeparam>
    public class JsonEnumToStringConvert<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

    }

    /// <summary>
    /// 将 Enum 类型 ToString()
    /// </summary>
    public class JsonEnumToStringConvert : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var types = objectType.GetGenericArguments();
            return (objectType.IsEnum || (types.Length > 0 && types[0].IsEnum)) && objectType != typeof(ApiResultCode);
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

            writer.WriteValue(value.ToString());
        }

    }
}