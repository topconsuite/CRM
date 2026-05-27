using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.CentroCusto
{
    public class CentroCustoResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "fix")]
        public bool Fixo { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool Ativo { get; set; }
    }
}
