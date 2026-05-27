using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.Funcionario
{
    public class FuncionarioAdicionarRequest
    {
        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::active")]
        [JsonProperty(PropertyName = "active")]
        public bool Ativo { get; set; } = true;

        [JsonProperty(PropertyName = "buyer")]
        public bool Comprador { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::concrete_batching_plant")]
        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int Usina { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::cpf")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH_WITH_MINIMUM) + "::cpf::11::14")]
        [JsonProperty(PropertyName = "cpf")]
        public string CPF { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::department")]
        [JsonProperty(PropertyName = "department")]
        public int Departamento { get; set; }

        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::entry_time")]
        [JsonProperty(PropertyName = "entry_time")]
        public string HoraEntrada { get; set; }

        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::exit_time")]
        [JsonProperty(PropertyName = "exit_time")]
        public string HoraSaida { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::external_id")]
        [StringLength(100, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::external_id::100")]
        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }

        [JsonProperty(PropertyName = "hourly_rate")]
        public double ValorHora { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::name")]
        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::name::40")]
        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::re")]
        [JsonProperty(PropertyName = "re")]
        public int RE { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::role")]
        [JsonProperty(PropertyName = "role")]
        public int Funcao { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::shortened_name")]
        [StringLength(15, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::shortened_name::15")]
        [JsonProperty(PropertyName = "shortened_name")]
        public string NomeReduzido { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_REQUIRED) + "::system_user")]
        [StringLength(10, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::system_user::10")]
        [JsonProperty(PropertyName = "system_user")]
        public string UsuarioSistema { get; set; }
    }
}
