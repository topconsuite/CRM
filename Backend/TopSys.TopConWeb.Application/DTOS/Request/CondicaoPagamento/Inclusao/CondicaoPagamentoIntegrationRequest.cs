using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Constants.CondicaoPagamento;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao
{
    public class CondicaoPagamentoIntegrationRequest
    {
        [Required]
        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Descricao)]
        public string Descricao { get; set; }

        [Required]
        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.QuantidadeParcelas)]
        public int? QuantidadeParcelas { get; set; }

        [Required]
        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento1Parcela)]
        public int? Vencimento1Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento2Parcela)]
        public int? Vencimento2Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento3Parcela)]
        public int? Vencimento3Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento4Parcela)]
        public int? Vencimento4Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento5Parcela)]
        public int? Vencimento5Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento6Parcela)]
        public int? Vencimento6Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento7Parcela)]
        public int? Vencimento7Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento8Parcela)]
        public int? Vencimento8Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento9Parcela)]
        public int? Vencimento9Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento10Parcela)]
        public int? Vencimento10Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento11Parcela)]
        public int? Vencimento11Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.Vencimento12Parcela)]
        public int? Vencimento12Parcela { get; set; }

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.CondicaoDaCobrancaCod)]
        public char CondicaoDaCobrancaCod { get; set; } = 'E';

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.VencimentoFixoSemana)]
        public bool VencimentoFixoSemana { get; set; } = false;

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.DiaUtilDeVencimento)]
        public ECondicaoPagamentoDiaUltimoVencimento DiaUtilDeVencimento { get; set; } = ECondicaoPagamentoDiaUltimoVencimento.MaintainExpiration;

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.IdExterno)]
        public string IdExterno { get; set; } = string.Empty;

        [JsonProperty(PropertyName = CondicaoPagamentoIntegrationAttributesNames.TiposDeCobrancaCodigos)]
        public List<int> TiposDeCobrancaCodigos { get; set; } = new List<int>();
    }
}
