using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral
{
    public class FuncionarioDepartamentoRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::employeeDepartments")]
        [UniqueFields(nameof(CadastroGeralIntegracaoAdicionarRequest.ExternalId), ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_UNIQUE_FIELDS) + "::(external_id)")]
        [MaxListLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_MAX_LIST_LENGTH) + "::100")]
        [JsonProperty(PropertyName = "employeeDepartments")]
        public CadastroGeralIntegracaoAdicionarRequest[] FuncionariosDepartamentos { get; set; }
    }
}
