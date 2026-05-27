using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Application.CustomValidations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;


namespace TopSys.TopConWeb.Application.DTOS.Request.Fatura
{
    [AtLeastOnePropertyRequired]
    public class FaturaAtualizarRequest
    {
        [Range(0, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::rps_number" + "::6")]
        [JsonProperty(PropertyName = "rps_number")]
        public int? NumeroRps { get; set; }
        
        [Range(0, 999999999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::service_invoice_number" + "::15")]
        [JsonProperty(PropertyName = "service_invoice_number")]
        public long? NumeroNfse { get; set; }
        
    }
}