using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;

namespace TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral
{
    public class EquipamentoTipoRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::equipmentTypes")]
        [UniqueFields(nameof(CadastroGeralIntegracaoAdicionarRequest.ExternalId), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_FIELDS) + "::(external_id)")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_MAX_LIST_LENGTH) + "::100")]
        [JsonProperty(PropertyName = "equipmentTypes")]
        public CadastroGeralIntegracaoAdicionarRequest[] EquipamentoTipos { get; set; }
    }
}
