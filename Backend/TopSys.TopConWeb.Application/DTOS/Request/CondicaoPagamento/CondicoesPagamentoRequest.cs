using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento
{
    public class CondicoesPagamentoRequest
    {
        [Required(ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::paymentConditions")]
        [MaxListLength(100,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) +
                           "::paymentConditions" + "::100")]
        [JsonProperty(PropertyName = "paymentConditions")]
        public CondicoesPagamentoAdicionarRequest[] CondicoesPagamento { get; set; }
    }
}