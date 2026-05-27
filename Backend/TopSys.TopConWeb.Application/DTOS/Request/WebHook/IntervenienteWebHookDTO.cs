using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook
{

    public class IntervenienteAddressWebHookDTO
    {
        [JsonProperty("street")]
        public string EnderecoLogradouro { get; set; }

        [JsonProperty("number")]
        public string EnderecoNumero { get; set; }

        [JsonProperty("complement")]
        public string EnderecoComplemento { get; set; } 

        [JsonProperty("neighborhood")]
        public string EnderecoBairro { get; set; } 

        [JsonProperty("city")]
        public string Cidade { get; set; }

        [JsonProperty("state")]
        public string Estado { get; set; }

        [JsonProperty("zip_code")]
        public string EnderecoCep { get; set; } 

        [JsonProperty("country")]
        public string Pais { get; set; } = "BR"; 
    }


    public class IntervenienteWebHookDTO
    {
        [JsonProperty("client_id")]
        public string Codigo { get; set; } 

        [JsonProperty("name")]
        public string Nome { get; set; }

        [JsonProperty("legal_name")]
        public string Razao { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("charging_email")]
        public string EmailCobranca { get; set; }

        [JsonProperty("phone")]
        public string Telefone { get; set; }

        [JsonProperty("document")]
        public string CpfCnpj { get; set; }

        [JsonProperty("state_registration")]
        public string InscricaoEstadual { get; set; }

        [JsonProperty("external_id")]
        public string IdExterno { get; set; }

        [JsonProperty("address")]
        public IntervenienteAddressWebHookDTO Endereco { get; set; }
    }

}
