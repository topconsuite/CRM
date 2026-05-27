using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral
{
    public class FuncionarioDepartamentoRequest
    {
        [Required(ErrorMessage = "The departments employees is required")]
        [UniqueCombinations("Codigo", ErrorMessage = "Duplicate code in the list.")]
        [UniqueFields("ExternalId", ErrorMessage = "Duplicate external id in the list.")]
        [MaxListLength(100, ErrorMessage = "The list cannot have more than 100 elements.")]
        [JsonProperty(PropertyName = "departmentsEmployees")]
        public CadastroGeralIntegracaoAdicionarRequest[] FuncionariosDepartamentos { get; set; }
    }
}
