using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoIntegracao
{
    public class PagamentosRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::concrete_batching_plant_contract")]
        [JsonProperty(PropertyName = "concrete_batching_plant_contract")]
        public int UsinaCodigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_number")]
        [JsonProperty(PropertyName = "contract_number")]
        public int ContratoNumero { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_year")]
        [JsonProperty(PropertyName = "contract_year")]
        public int ContratoAno { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::contract_payments")]
        [UniqueFields(nameof(PagamentoAprovarRequest.Sequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_FIELDS) + "::(sequence)")]
        [JsonProperty(PropertyName = "contract_payments")]
        public PagamentoAprovarRequest[] Pagamentos { get; set; }
    }
}
