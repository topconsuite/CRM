using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class TipoDeCobrancaDTO
    {
        [JsonProperty(PropertyName = "billing_type_code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "payment_method")]
        public string Forma { get; set; }

        [JsonProperty(PropertyName = "bill_collector")]
        public int Portador { get; set; }

        [JsonProperty(PropertyName = "situation")]
        public int Situacao { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }
    }
}
