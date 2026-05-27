using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.TipoDeCobranca
{
    public class TipoDeCobrancaAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Code")]
        [Range(500, 599, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN) + "::Code" + "::499" + "::600")]
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::payment_form")]
        [MaxLength(2, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::payment_form" + "::2")]
        [JsonProperty(PropertyName = "payment_form")]
        public string Forma { get; set; }
        
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Carrier")]
        [Range(1, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Carrier" + "::3")]
        [JsonProperty(PropertyName = "carrier")]
        public int Portador { get; set; }

        [JsonProperty(PropertyName = "situation")]
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::situation" + "::2")]
        public int? Situacao { get; set; }
        
        [JsonProperty(PropertyName = "mandatory")]
        public bool? Obrigatorio { get; set; }
        
        [JsonProperty(PropertyName = "approval")]
        public bool? Aprovacao { get; set; }
        
        [JsonProperty(PropertyName = "fix")]
        public bool? Fixo { get; set; }
        
        [JsonProperty(PropertyName = "utilization_accounts_payable")]
        public bool? UtilCap { get; set; }
    }
}