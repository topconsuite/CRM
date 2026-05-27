using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.TipoDocumento
{
    public class TipoDocumentoRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::DocumentType")]
        [UniqueCombinations(nameof(TipoDocumentoAdicionarRequest.Codigo),
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + "::Code" +
                           "::DocumentType")]
        [MaxListLength(100,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) +
                           "::DocumentType" + "::100")]
        [JsonProperty(PropertyName = "DocumentType")]
        public TipoDocumentoAdicionarRequest[] TiposDocumento { get; set; }
    }
}