using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Application.CustomValidations;


namespace TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral
{
    public class FuncionarioStatusRequest
    {
        [Required(ErrorMessage = "The status employees is required")]
        [UniqueCombinations("Codigo", ErrorMessage = "Duplicate code in the list.")]
        [UniqueFields("ExternalId", ErrorMessage = "Duplicate external id in the list.")]
        [MaxListLength(100, ErrorMessage = "The list cannot have more than 100 elements.")]
        [JsonProperty(PropertyName = "statusEmployees")]
        public CadastroGeralIntegracaoAdicionarRequest[] FuncionariosStatus { get; set; }
    }
}
