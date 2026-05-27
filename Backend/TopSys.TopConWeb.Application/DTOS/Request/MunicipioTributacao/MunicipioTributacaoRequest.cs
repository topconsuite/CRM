using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao
{
    public class MunicipioTributacaoRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::municipal taxes")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) + "::municipal taxes" + "::100")]
        [JsonProperty(PropertyName = "municipalTaxes")]
        public MunicipioTributacaoAdicionarRequest[] MunicipiosTributacao { get; set; }
    }
}


