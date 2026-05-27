using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.PortadorCobranca
{
    public class PortadorCobrancaResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }
        
        [JsonProperty(PropertyName = "bank_code")]
        public int ContaCodigo { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "issue_an_invoice")]
        public string EmiteCobranca { get; set; }

        [JsonProperty(PropertyName = "company")]
        public int ContaEmpresaCodigo { get; set; }
        
        [JsonProperty(PropertyName = "situation")]
        public int Situacao { get; set; }
    }
}