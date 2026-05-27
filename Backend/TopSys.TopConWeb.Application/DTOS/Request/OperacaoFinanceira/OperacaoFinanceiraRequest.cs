using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.OperacaoFinanceira
{
    public class OperacaoFinanceiraRequest
    {
        [Required(ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::FinanceOperation")]
        [UniqueCombinations(nameof(OperacaoFinanceiraAdicionarRequest.Codigo),
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + 
                           "::Code" + "::FinanceOperation")]
        [MaxListLength(100,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) +
                           "::Finance Operation" + "::100")]
        [JsonProperty(PropertyName = "FinanceOperation")]
        public OperacaoFinanceiraAdicionarRequest[] TiposDeCobranca { get; set; }
    }
}