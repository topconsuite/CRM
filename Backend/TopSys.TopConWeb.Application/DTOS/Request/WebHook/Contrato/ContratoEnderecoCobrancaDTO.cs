using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook.Contrato
{
    public class ContratoEnderecoCobrancaDTO
    {
        public ContratoEnderecoCobrancaDTO(string logradouro, string numero, string complemento, string bairro, int? municipioCodigo, string municipio, string uf, string cep)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            MunicipioCodigo = municipioCodigo;
            Municipio = municipio;
            Uf = uf;
            Cep = cep;
        }

        public ContratoEnderecoCobrancaDTO() { }

        [JsonProperty(PropertyName = "street")]
        public string Logradouro { get; set; } = "";

        [JsonProperty(PropertyName = "number")]
        public string Numero { get; set; } = "";

        [JsonProperty(PropertyName = "complement")]
        public string Complemento { get; set; } = "";

        [JsonProperty(PropertyName = "neighborhood")]
        public string Bairro { get; set; } = "";

        [JsonProperty(PropertyName = "city_code")]
        public int? MunicipioCodigo { get; set; } = 0;

        [JsonProperty(PropertyName = "city")]
        public string Municipio { get; set; } = "";

        [JsonProperty(PropertyName = "state")]
        public string Uf { get; set; } = "";

        [JsonProperty(PropertyName = "zip_code")]
        public string Cep { get; set; } = "";
    }
}
