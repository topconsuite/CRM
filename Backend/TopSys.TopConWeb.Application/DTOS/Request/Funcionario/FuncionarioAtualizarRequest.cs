using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.CustomValidations;

namespace TopSys.TopConWeb.Application.DTOS.Request.Funcionario
{
    [AtLeastOnePropertyRequired]
    public class FuncionarioAtualizarRequest
    {
        [JsonProperty(PropertyName = "active")]
        public bool? Ativo { get; set; }

        [JsonProperty(PropertyName = "buyer")]
        public bool? Comprador { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int? Usina { get; set; }

        [JsonProperty(PropertyName = "cpf")]
        public string CPF { get; set; }

        [JsonProperty(PropertyName = "department")]
        public int? Departamento { get; set; }

        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::entry_time")]
        [JsonProperty(PropertyName = "entry_time")]
        public string HoraEntrada { get; set; }

        [ValidTimeFormat(ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_VALID_TIME_FORMAT) + "::exit_time")]
        [JsonProperty(PropertyName = "exit_time")]
        public string HoraSaida { get; set; }

        [JsonProperty(PropertyName = "hourly_rate")]
        public double? ValorHora { get; set; }

        [StringLength(40, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::name::40")]
        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "re")]
        public int? RE { get; set; }

        [JsonProperty(PropertyName = "role")]
        public int? Funcao { get; set; }

        [StringLength(15, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::shortened_name::15")]
        [JsonProperty(PropertyName = "shortened_name")]
        public string NomeReduzido { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int? Status { get; set; }

        [StringLength(10, ErrorMessage = nameof(EResourcesDataAnnotationsMessages.DATA_ANNOTATIONS_MESSAGES_STRING_LENGTH) + "::system_user::10")]
        [JsonProperty(PropertyName = "system_user")]
        public string UsuarioSistema { get; set; }
    }
}
