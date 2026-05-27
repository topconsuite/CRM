using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.Vendedor
{
    public class VendedorIntegracaoResponse
    {
        [JsonProperty(PropertyName = "active")]
        public string Ativo { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string EnderecoLogradouro { get; set; }
        [JsonProperty(PropertyName = "cell_phone")]
        public int Celular { get; set; }
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }
        [JsonProperty(PropertyName = "company_name")]
        public string RazaoSocial { get; set; }
        [JsonProperty(PropertyName = "complement")]
        public string Complemento { get; set; }
        [JsonProperty(PropertyName = "concrete_batching_plant")]
        public int Usina { get; set; }
        [JsonProperty(PropertyName = "county")]
        public int Municipio { get; set; }
        [JsonProperty(PropertyName = "ddd_cell_phone")]
        public int DDDCelular { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "external_id")]
        public string ExternalId { get; set; }
        [JsonProperty(PropertyName = "godfather_seller")]
        public int VendedorPadrinho { get; set; }
        [JsonProperty(PropertyName = "intervener")]
        public int Interveniente { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Nome { get; set; }
        [JsonProperty(PropertyName = "number")]
        public int EnderecoNumero { get; set; }
        [JsonProperty(PropertyName = "payment_terms")]
        public int CondicaoPagamento { get; set; }
        [JsonProperty(PropertyName = "re")]
        public int Re { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Funcao { get; set; }
        //[JsonProperty(PropertyName = "state")]
        //public string Estado { get; set; }
        [JsonProperty(PropertyName = "system_user")]
        public string Usuario { get; set; }
        [JsonProperty(PropertyName = "zip_code")]
        public string Cep { get; set; }
    }
}
