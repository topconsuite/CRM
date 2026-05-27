using Newtonsoft.Json;
namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class ChaveTituloContasAReceberDTO
    {
        [JsonProperty(PropertyName = "company")]
        public int EmpresaCodigo { get; set; }
        
        [JsonProperty(PropertyName = "document_type")]
        public int DocumentoTipoCodigo { get; set; }
        
        [JsonProperty(PropertyName = "document_serie")]
        public string DocumentoSerie { get; set; }
        
        [JsonProperty(PropertyName = "document_number")]
        public long DocumentoNumero { get; set; }
        
        [JsonProperty(PropertyName = "sequence")]
        public int DocumentoSequencia { get; set; }
        
        [JsonProperty(PropertyName = "bank_brand_code")]
        public int BancoCodigoOficial { get; set; }
        
        [JsonProperty(PropertyName = "agency_number")]
        public int BancoNumeroAgencia { get; set; }
        
        [JsonProperty(PropertyName = "account_number")]
        public int BancoNumeroConta { get; set; }
        
        [JsonProperty(PropertyName = "account_verification_digit")]
        public int BancoDvConta { get; set; }
        
        [JsonProperty(PropertyName = "splitting")]
        public int Desdobramento { get; set; }
    }
}