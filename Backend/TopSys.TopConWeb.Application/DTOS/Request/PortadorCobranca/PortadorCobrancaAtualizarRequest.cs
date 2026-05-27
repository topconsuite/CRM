using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.PortadorCobranca

{
    public class PortadorCobrancaAtualizarRequest
    {
        
        [Range(1, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Code" + "::3")]
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }
        
        [Range(1, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Bank_code" + "::3")]
        [JsonProperty(PropertyName = "bank_code")]
        public int? ContaCodigo { get; set; }
        
        [MaxLength(25, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Description" + "::25")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }
        
        [JsonProperty(PropertyName = "issue_an_invoice")]
        public bool? EmiteCobranca { get; set; }
        
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Company" + "::2")]
        [JsonProperty(PropertyName = "company")]
        public int? ContaEmpresaCodigo { get; set; }
        
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Situation" + "::2")]
        [JsonProperty(PropertyName = "situation")]
        public int? Situacao { get; set; }
    }
}