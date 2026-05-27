using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.PortadorCobranca
{
    public class PortadorCobrancaRequest
    {
        [Required(ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::billCollector")]
        [UniqueCombinations(nameof(PortadorCobrancaAdicionarRequest.Codigo),
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + "::Code" +
                           "::billCollector")]
        [MaxListLength(100,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) +
                           "::billCollector" + "::100")]
        [JsonProperty(PropertyName = "billCollector")]
        public PortadorCobrancaAdicionarRequest[] PortadoresCobranca { get; set; }
    }
}