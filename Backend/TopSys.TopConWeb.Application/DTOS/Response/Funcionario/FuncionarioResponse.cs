using Newtonsoft.Json;
using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Funcionario
{
    public class FuncionarioResponse
    {
        [JsonProperty(PropertyName = "active")]
        public bool Ativo { get; set; } = true;

        [JsonProperty(PropertyName = "buyer")]
        public bool Comprador { get; set; }

        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int Usina { get; set; }

        [JsonProperty(PropertyName = "cpf")]
        public string CPF { get; set; }

        [JsonProperty(PropertyName = "department")]
        public int Departamento { get; set; }

        [JsonProperty(PropertyName = "entry_time")]
        public string HoraEntrada { get; set; }

        [JsonProperty(PropertyName = "exit_time")]
        public string HoraSaida { get; set; }

        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }

        [JsonProperty(PropertyName = "hourly_rate")]
        public int ValorHora { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "re")]
        public int RE { get; set; }

        [JsonProperty(PropertyName = "role")]
        public int Funcao { get; set; }

        [JsonProperty(PropertyName = "shortened_name")]
        public string NomeReduzido { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "system_user")]
        public string UsuarioSistema { get; set; }

        [JsonProperty(PropertyName = "intervener")]
        public int IntervenienteCodigo { get; set; } = 0;

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; } = "";
    }
}
