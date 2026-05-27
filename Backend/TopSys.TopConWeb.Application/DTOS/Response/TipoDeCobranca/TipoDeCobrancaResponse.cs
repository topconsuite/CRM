using Newtonsoft.Json;  

namespace TopSys.TopConWeb.Application.DTOS.Response.TipoDeCobranca
{
    public class TipoDeCobrancaResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "payment_form")]
        public string Forma { get; set; }

        [JsonProperty(PropertyName = "carrier")]
        public int Portador { get; set; }

        [JsonProperty(PropertyName = "situation")]
        public int Situacao { get; set; }
        
        [JsonProperty(PropertyName = "mandatory")]
        public string Obrigatorio { get; set; }
        
        [JsonProperty(PropertyName = "approval")]
        public string Aprovacao { get; set; }
        
        [JsonProperty(PropertyName = "fix")]
        public string Fixo { get; set; }
        
        [JsonProperty(PropertyName = "utilization_accounts_payable")]
        public string UtilCap { get; set; }
    }
}