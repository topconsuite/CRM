using Newtonsoft.Json;
using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital
{
    public class NotaFiscalDigitalItemComplementoResponse
    {
        [JsonProperty(PropertyName = "cbs_tax_id")]
        public int IdImpostoCBS { get; set; }

        [JsonProperty(PropertyName = "is_tax_id")]
        public int IdImpostoIS { get; set; }

        [JsonProperty(PropertyName = "ibs_tax_id")]
        public int IdImpostoIBS { get; set; }

        [JsonProperty(PropertyName = "tax_situation_cbs_ibs")]
        public string CST_CBS_IBS { get; set; }

        [JsonProperty(PropertyName = "tax_classification_cbs_ibs")]
        public string ClassificacaoTributariaCBSIBS { get; set; }

        [JsonProperty(PropertyName = "tax_situation_is")]
        public string CST_IS { get; set; }

        [JsonProperty(PropertyName = "tax_classification_is")]
        public string ClassificacaoTributariaIS { get; set; }

        [JsonProperty(PropertyName = "base_cbs_ibs")]
        public decimal BaseIBSCBS { get; set; }

        [JsonProperty(PropertyName = "effective_cbs_rate")]
        public decimal AliquotaCBSEfetiva { get; set; }

        [JsonProperty(PropertyName = "nominal_cbs_rate")]
        public decimal AliquotaCBS { get; set; }

        [JsonProperty(PropertyName = "cbs_reduction_percentage")]
        public decimal PercentualReducaoCBS { get; set; }

        [JsonProperty(PropertyName = "value_cbs")]
        public decimal ValorCBS { get; set; }

        [JsonProperty(PropertyName = "effective_ibs_municipal_rate")]
        public decimal AliquotaIBSMunicipalEfetiva { get; set; }

        [JsonProperty(PropertyName = "nominal_ibs_municipal_rate")]
        public decimal AliquotaIBSMunicipal { get; set; }

        [JsonProperty(PropertyName = "ibs_municipal_reduction_percentage")]
        public decimal PercentualReducaoIBSMunicipal { get; set; }

        [JsonProperty(PropertyName = "value_ibs_municipal")]
        public decimal ValorIBSMunicipal { get; set; }

        [JsonProperty(PropertyName = "effective_ibs_state_rate")]
        public decimal AliquotaIBSEstadualEfetiva { get; set; }

        [JsonProperty(PropertyName = "nominal_ibs_state_rate")]
        public decimal AliquotaIBSEstadual { get; set; }

        [JsonProperty(PropertyName = "ibs_state_reduction_percentage")]
        public decimal PercentualReducaoIBSEstadual { get; set; }

        [JsonProperty(PropertyName = "value_ibs_state")]
        public decimal ValorIBSEstadual { get; set; }

        [JsonProperty(PropertyName = "value_ibs")]
        public decimal ValorIBS { get; set; }

        [JsonProperty(PropertyName = "base_is")]
        public decimal BaseIS { get; set; }

        [JsonProperty(PropertyName = "nominal_is_rate")]
        public decimal AliquotaIS { get; set; }

        [JsonProperty(PropertyName = "value_is")]
        public decimal ValorIS { get; set; }

        [JsonProperty(PropertyName = "dfe_reference_sequence")]
        public short SequenciaDFERef { get; set; }

        [JsonProperty(PropertyName = "dfe_reference_key")]
        public string ChaveNFeDFERef { get; set; }
    }
}
