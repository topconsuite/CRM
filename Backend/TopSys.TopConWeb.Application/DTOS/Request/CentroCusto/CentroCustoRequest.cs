using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.CentroCusto
{
    public class CentroCustoRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::financial cost center")]
        [UniqueCombinations(nameof(CentroCustoAdicionarRequest.Codigo), ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + "::code" + "::financial cost center")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) + "::financial cost center" + "::100")]
        [JsonProperty(PropertyName = "financialCostCenter")]
        public CentroCustoAdicionarRequest[] CentrosCusto { get; set; }
    }
}
