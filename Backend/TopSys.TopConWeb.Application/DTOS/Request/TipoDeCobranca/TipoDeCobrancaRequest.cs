using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.TipoDeCobranca
{
    public class TipoDeCobrancaRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::BillingType")]
        [UniqueCombinations(nameof(TipoDeCobrancaAdicionarRequest.Codigo),
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + "::Code" +
                           "::BillingType")]
        [MaxListLength(100,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) +
                           "::BillingType" + "::100")]
        [JsonProperty(PropertyName = "BillingType")]
        public TipoDeCobrancaAdicionarRequest[] TiposDeCobranca { get; set; }
    }
}