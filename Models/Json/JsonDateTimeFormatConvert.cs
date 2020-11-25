using System;

namespace Newtonsoft.Json
{

    public class JsonDateTimeFormatConvert : JsonConverter
    {
        private readonly string _dateTimeFormat;
        private readonly string _dateFormat;

        public JsonDateTimeFormatConvert(string dateTimeFormat = "yyyy-MM-dd HH:mm:ss", string dateFormat = "yyyy-MM-dd")
        {
            _dateTimeFormat = dateTimeFormat;
            _dateFormat = dateFormat;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var dt = (DateTime)value;
            if (dt.Hour == 0 && dt.Minute == 0 && dt.Second == 0)
            {
                writer.WriteValue(dt.ToString(_dateFormat));
            }
            else
            {
                writer.WriteValue(dt.ToString(_dateTimeFormat));
            }
        }

    }
}