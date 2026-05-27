using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.Remessa
{
    public class RemessaCancelarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::cancel_packing_list")]
        [JsonProperty(PropertyName = "cancel_packing_list")]
        public bool? Cancelar { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::discarded_concrete")]
        [JsonProperty(PropertyName = "discarded_concrete")]
        public bool? DiscartarConcreto { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::dosed_concrete")]
        [JsonProperty(PropertyName = "dosed_concrete")]
        public bool? ConcretoJaDosado { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::reuse_weighing_in_the_invoice")]
        [JsonProperty(PropertyName = "reuse_weighing_in_the_invoice")]
        public int? NotaFiscalParaSerUsadaNoReaproveitamento { get; set; }
    }
}
