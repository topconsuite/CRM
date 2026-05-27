using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;

namespace TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao
{
    public class ProgramacoesRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::concretings")]
        [UniqueCombinations(nameof(ProgramacaoAdicionarRequest.UsinaCodigo), nameof(ProgramacaoAdicionarRequest.ContratoNumero), nameof(ProgramacaoAdicionarRequest.ContratoAno), nameof(ProgramacaoAdicionarRequest.ObraTracoSequencia), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_COMBINATIONS) + "::(concrete_batching_plant_contract + contract_year + contract_number + concreting_sequence)")]
        [UniqueFields(nameof(ProgramacaoAdicionarRequest.ExternalId), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_FIELDS) + "::(external_id)")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_MAX_LIST_LENGTH) + "::100")]
        [JsonProperty(PropertyName = "concretings")]
        public ProgramacaoAdicionarRequest[] Programacoes { get; set; }
    }

}
