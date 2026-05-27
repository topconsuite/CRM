using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.PortadorCobranca
{
    public class PortadorCobrancaAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Code")]
        [Range(1, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::Code" + "::3")]
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::bank_code")]
        [Range(1, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::bank_code" + "::3")]
        [JsonProperty(PropertyName = "bank_code")]
        public int ContaCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Description")]
        [MaxLength(25,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::Description" + "::25")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [Required(ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::issue_an_invoice")]
        [JsonProperty(PropertyName = "issue_an_invoice")]
        public bool? EmiteCobranca { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Company")]
        [Range(1, 99,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::Company" + "::2")]
        [JsonProperty(PropertyName = "company")]
        public int ContaEmpresaCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Situation")]
        [Range(1, 99,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::Situation" + "::2")]
        [JsonProperty(PropertyName = "situation")]
        public int Situacao { get; set; }
    }
}