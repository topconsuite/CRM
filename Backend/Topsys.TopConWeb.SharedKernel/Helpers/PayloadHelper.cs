using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class PayloadHelper
    {

        private static string SerializeWithUtf8(object obj, JsonSerializerSettings settings)
        {
            var serializer = JsonSerializer.Create(settings);
            using (var sw = new StringWriter(new StringBuilder(), System.Globalization.CultureInfo.InvariantCulture))
            using (var writer = new JsonTextWriter(sw) { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii })
            {
                serializer.Serialize(writer, obj);
                return sw.ToString();
            }
        }

        public static string ConvertToJson(object payload) => SerializeWithUtf8(payload, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented });


        public static string ConvertToJson(object oldObject, object newObject) {

            var payload = new
            {
                oldObject,
                newObject
            };

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return SerializeWithUtf8(payload, settings);

        }

    }
}
