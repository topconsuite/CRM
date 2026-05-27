using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.SituacaoFinanceira
{
    public class SituacaoFinanceiraRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::finance situation")]
        [UniqueCombinations(nameof(SituacaoFinanceiraAdicionarRequest.Codigo), ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + "::code" + "::finance situation")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) + "::finance situation" + "::100")]
        [JsonProperty(PropertyName = "financeSituation")]
        public SituacaoFinanceiraAdicionarRequest[] SituacoesFinanceiras { get; set; }
    }
}
