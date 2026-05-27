using Newtonsoft.Json;
using TopSys.TopConWeb.Application.CustomValidations;

namespace TopSys.TopConWeb.Application.DTOS.Request.Prensa
{
    [AtLeastOnePropertyRequired]
    public class PrensaRequest
    {
        [JsonProperty(PropertyName = "press_name")]
        public string PrensaNome { get; set; } = "";

        [JsonProperty(PropertyName = "pressure")]
        public string Carga { get; set; } = "";
    }
}
