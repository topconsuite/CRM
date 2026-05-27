using Newtonsoft.Json;
using System;


namespace Topsys.TopConWeb.SharedKernel.Helpers
{

    public class DateTimeToDateJsonConverter : JsonConverter
    {
        private readonly string _format;

        public DateTimeToDateJsonConverter(string format)
        {
            _format = format;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime dateTime)
            {
                writer.WriteValue(DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime().ToString(_format));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }
    }
}