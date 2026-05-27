using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.TipoDocumento
{
    public class TipoDocumentoAtualizarRequest
    {
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::code" + "::2")]
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }
        
        [MaxLength(4, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::abbreviation" + "::4")]
        [JsonProperty(PropertyName = "abbreviation")]
        public string Abreviacao { get; set; }
        
        [MaxLength(22, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::description" + "::22")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "fixed")]
        public bool? Fixo { get; set; }
        
        [JsonProperty(PropertyName = "document_model")]
        [MaxLength(2, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::document_model" + "::2")]
        public string ModDoc { get; set; }
        
        [JsonProperty(PropertyName = "eletronic_service_invoice")]
        public bool? Nfse { get; set; }
        
        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::service_document_type" + "::2")]
        [JsonProperty(PropertyName = "service_document_type")]
        public int? TpDocServ { get; set; }
        
    }
}