using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.CentroCusto
{
    public class CentroCustoAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Code")]
        [Range(1, 99999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Code" + "::5")]
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Description")]
        [MaxLength(20, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Description" + "::20")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Fix")]
        [JsonProperty(PropertyName = "fix")]
        public bool? Fixo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Active")]
        [JsonProperty(PropertyName = "active")]
        public bool? Ativo { get; set; }
    }
}
