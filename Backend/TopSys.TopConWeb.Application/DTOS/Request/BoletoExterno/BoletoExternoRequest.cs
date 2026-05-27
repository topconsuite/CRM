using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.BoletoExterno
{
    public class BoletoExternoRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::external bank slip")]
        //[UniqueCombinations(nameof(BoletoExternoAdicionarRequest.Codigo), ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST) + "::code" + "::external bank slip")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_ELEMENTS_THAN_ALLOWED) + "::external bank slip" + "::100")]
        [JsonProperty(PropertyName = "externalBankSlip")]
        public BoletoExternoAdicionarRequest[] BoletosExternos { get; set; }
    }
}
