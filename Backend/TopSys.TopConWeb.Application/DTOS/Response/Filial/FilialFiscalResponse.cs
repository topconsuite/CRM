using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.Filial
{
    public class FilialFiscalResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "trade_name")]
        public string Nome { get; set; } 

        [JsonProperty(PropertyName = "company_name")]
        public string RazaoSocial { get; set; } 

        [JsonProperty(PropertyName = "cep_code")]
        public string EnderecoCep { get; set; } 

        [JsonProperty(PropertyName = "street_address")]
        public string EnderecoLogradouro { get; set; } 

        [JsonProperty(PropertyName = "number")]
        public int? EnderecoNumero { get; set; }

        [JsonProperty(PropertyName = "complement")]
        public string EnderecoComplemento { get; set; } 

        [JsonProperty(PropertyName = "neighborhood_address")]
        public string EnderecoBairro { get; set; } 

        [JsonProperty(PropertyName = "internal_municipal_code")]
        public int? EnderecoMunicipioCodigo { get; set; } 

        [JsonProperty(PropertyName = "cnpj")]
        public string Cnpj { get; set; } 

        [JsonProperty(PropertyName = "state_registration")]
        public string InscricaoEstadual { get; set; } 

        [JsonProperty(PropertyName = "municipal_registration")]
        public string InscricaoMunicipal { get; set; } 

        [JsonProperty(PropertyName = "cost_center")]
        public int? CentroCusto { get; set; }
    }
}
