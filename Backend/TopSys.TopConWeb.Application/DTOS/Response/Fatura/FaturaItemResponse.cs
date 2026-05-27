using Newtonsoft.Json;
using System;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.Fatura
{
    public class FaturaItemResponse
    {
        [JsonProperty(PropertyName = "item_sequence")]
        public int SequenciaItem { get; set; }

        [JsonProperty(PropertyName = "packing_list_branch")]
        public int FilialNf { get; set; }

        [JsonProperty(PropertyName = "packing_list_document_type")]
        public int TipoDocumentoNf { get; set; }

        [JsonProperty(PropertyName = "packing_list_number")]
        public long NumeroNf { get; set; }

        [JsonProperty(PropertyName = "packing_list_series")]
        public string SerieNf { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "packing_list_date")]
        public DateTime DataNf { get; set; }

        [JsonProperty(PropertyName = "packing_list_billing_concrete_batching_plant")]
        public int UsinaFaturamentoNf { get; set; }

        [JsonProperty(PropertyName = "packing_list_weighing_concrete_batching_plant")]
        public int UsinaPesagemNf { get; set; }

        [JsonProperty(PropertyName = "dash_code")]
        public string CodigoTraco { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string DescricaoProduto { get; set; }
        
        [JsonProperty(PropertyName = "external_id")]
        public string IdExternoMercadoria { get; set; }
        
        [JsonProperty(PropertyName = "product_number")]
        public int NumeracaoProduto { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public string Unidade { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public float Quantidade { get; set; }

        [JsonProperty(PropertyName = "unit_price")]
        public float PrecoUnitario { get; set; }

        [JsonProperty(PropertyName = "total_price")]
        public float PrecoTotal { get; set; }

        [JsonProperty(PropertyName = "item_material_value")]
        public float ValorMaterial { get; set; }

        [JsonProperty(PropertyName = "item_gross_service_value")]
        public float ValorServicoBruto { get; set; }

        [JsonProperty(PropertyName = "item_service_value")]
        public float ValorServico { get; set; }

        [JsonProperty(PropertyName = "item_discount_value")]
        public float ValorDesconto { get; set; }

        [JsonProperty(PropertyName = "item_net_value")]
        public float ValorLiquido { get; set; }

        [JsonProperty(PropertyName = "item_pis_tax_status_code")]
        public string PisCodigoSituacaoTributaria { get; set; }

        [JsonProperty(PropertyName = "item_pis_calculation_basis")]
        public float PisBaseCalculo { get; set; }

        [JsonProperty(PropertyName = "item_pis_percentage")]
        public float PisPercentual { get; set; }

        [JsonProperty(PropertyName = "item_pis_value")]
        public float PisValor { get; set; }

        [JsonProperty(PropertyName = "item_cofins_tax_status_code")]
        public string CofinsCodigoSituacaoTributaria { get; set; }

        [JsonProperty(PropertyName = "item_cofins_calculation_basis")]
        public float CofinsBaseCalculo { get; set; }

        [JsonProperty(PropertyName = "item_cofins_percentage")]
        public float CofinsPercentual { get; set; }

        [JsonProperty(PropertyName = "item_cofins_value")]
        public float CofinsValor { get; set; }

        [JsonProperty(PropertyName = "contribution_taxation")]
        public string TributacaoContribuicao { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "initial_validity_of_contribution_taxation")]
        public DateTime InicioVigenciaTribContribuicao { get; set; }

        [JsonProperty(PropertyName = "item_withheld_pis_value")]
        public float ValorPisRetido { get; set; }

        [JsonProperty(PropertyName = "item_withheld_cofins_value")]
        public float ValorCofinsRetido { get; set; }

        [JsonProperty(PropertyName = "item_irrf_value")]
        public float ValorIrrf { get; set; }

        [JsonProperty(PropertyName = "item_withheld_csll_value")]
        public float ValorCsllRetido { get; set; }

        [JsonProperty(PropertyName = "item_iss_calculation_basis")]
        public float IssBaseCalculo { get; set; }

        [JsonProperty(PropertyName = "item_iss_percentage")]
        public float IssPercentual { get; set; }

        [JsonProperty(PropertyName = "item_iss_value")]
        public float IssValor { get; set; }

        [JsonProperty(PropertyName = "item_withheld_iss_value")]
        public float IssValorRetido { get; set; }

        [JsonProperty(PropertyName = "item_receipt_number")]
        public long NumeroRecibo { get; set; }

        [JsonProperty(PropertyName = "item_cost_center")]
        public int CentroCusto { get; set; }
        
        [JsonProperty(PropertyName = "cbs_tax_id")]
        public int IdImpostoCbs { get; set; }

        [JsonProperty(PropertyName = "ibs_tax_id")]
        public int IdImpostoIbs { get; set; }

        [JsonProperty(PropertyName = "tax_situation_cbs_ibs")]
        public string CstCbsIbs { get; set; }

        [JsonProperty(PropertyName = "tax_classification_cbs_ibs")]
        public string ClassificacaoTributariaCbsIbs { get; set; }
        
        [JsonProperty(PropertyName = "base_cbs_ibs")]
        public decimal BaseCbsIbs{ get; set; }

        [JsonProperty(PropertyName = "effective_cbs_rate")]
        public decimal AliquotaCbsEfetiva { get; set; }

        [JsonProperty(PropertyName = "nominal_cbs_rate")]
        public decimal AliquotaCbs { get; set; }

        [JsonProperty(PropertyName = "cbs_reduction_percentage")]
        public decimal PercentualReducaoCbs { get; set; }

        [JsonProperty(PropertyName = "value_cbs")]
        public decimal ValorCbs { get; set; }

        [JsonProperty(PropertyName = "effective_ibs_municipal_rate")]
        public decimal AliquotaIbsMunicipalEfetiva { get; set; }

        [JsonProperty(PropertyName = "nominal_ibs_municipal_rate")]
        public decimal AliquotaIbsMunicipal { get; set; }

        [JsonProperty(PropertyName = "ibs_municipal_reduction_percentage")]
        public decimal PercentualReducaoIbsMunicipal { get; set; }

        [JsonProperty(PropertyName = "value_ibs_municipal")]
        public decimal ValorIbsMunicipal { get; set; }

        [JsonProperty(PropertyName = "effective_ibs_state_rate")]
        public decimal AliquotaIbsEstadualEfetiva { get; set; }

        [JsonProperty(PropertyName = "nominal_ibs_state_rate")]
        public decimal AliquotaIbsEstadual { get; set; }

        [JsonProperty(PropertyName = "ibs_state_reduction_percentage")]
        public decimal PercentualReducaoIbsEstadual { get; set; }

        [JsonProperty(PropertyName = "value_ibs_state")]
        public decimal ValorIbsEstadual { get; set; }

        [JsonProperty(PropertyName = "value_ibs")]
        public decimal ValorIbs { get; set; }
    }
}
