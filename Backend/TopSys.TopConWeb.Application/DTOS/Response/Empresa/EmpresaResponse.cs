using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.Empresa
{
    public class EmpresaResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "trade_name")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "company_name")]
        public string Razao { get; set; }

        [JsonProperty(PropertyName = "cnpj_cpf")]
        public string CpfCnpj { get; set; }

        [JsonProperty(PropertyName = "state_registration")]
        public string InscricaoEstadual { get; set; }

        [JsonProperty(PropertyName = "street_address")]
        public string EnderecoLogradouro { get; set; }

        [JsonProperty(PropertyName = "number")]
        public int EnderecoNumero { get; set; }

        [JsonProperty(PropertyName = "neighborhood_address")]
        public string EnderecoBairro { get; set; }

        [JsonProperty(PropertyName = "municipal_code")]
        public int EnderecoMunicipioCodigo { get; set; }

        [JsonProperty(PropertyName = "cep_code")]
        public string EnderecoCep { get; set; }
    }
}
