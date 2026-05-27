using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.TipoDocumento
{
    public class TipoDocumentoResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }
        
        [JsonProperty(PropertyName = "abbreviation")]
        public string Abreviacao { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "fixed")]
        public bool? Fixo { get; set; }
        
        [JsonProperty(PropertyName = "document_model")]
        public string ModDoc { get; set; }
        
        [JsonProperty(PropertyName = "eletronic_service_invoice")]
        public bool? Nfse { get; set; }
        
        [JsonProperty(PropertyName = "service_document_type")]
        public int? TpDocServ { get; set; }
    }
}