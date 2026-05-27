using Newtonsoft.Json;
using System;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital
{
    public class NotaFiscalDigitalItemResponse
    {
        [JsonProperty(PropertyName = "item_sequence")]
        public int SequenciaItem { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "operation_date")]
        public DateTime DataOperacao { get; set; }

        [JsonProperty(PropertyName = "item_operation_transaction")]
        public int TransacaoDaOperacao { get; set; }

        [JsonProperty(PropertyName = "cfop")]
        public int Cfop { get; set; }

        [JsonProperty(PropertyName = "cfop_sequence")]
        public int SequenciaCfop { get; set; }

        [JsonProperty(PropertyName = "stock_type")]
        public int? TipoEstoque { get; set; }

        [JsonProperty(PropertyName = "product_code")]
        public string CódigoMercadoria { get; set; }
        
        [JsonProperty(PropertyName = "product_external_id")]
        public string IdExternoMercadoria { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public float Quantidade { get; set; }

        [JsonProperty(PropertyName = "unit_price")]
        public float PrecoUnitario { get; set; }

        [JsonProperty(PropertyName = "total_value")]
        public float ValorTotal { get; set; }

        [JsonProperty(PropertyName = "discount_value")]
        public float? ValorDesconto { get; set; }

        [JsonProperty(PropertyName = "freight_value")]
        public float? ValorFrete { get; set; }

        [JsonProperty(PropertyName = "insurance_value")]
        public float? ValorSeguro { get; set; }

        [JsonProperty(PropertyName = "other_expenses_value")]
        public float? ValorOutrasDespesas { get; set; }

        [JsonProperty(PropertyName = "tax_situation")]
        public string SituacaoTributaria { get; set; }

        [JsonProperty(PropertyName = "icms_calculation_basis")]
        public float? BaseCalculoIcms { get; set; }

        [JsonProperty(PropertyName = "icms_aliquot")]
        public float? AliquotaIcms { get; set; }

        [JsonProperty(PropertyName = "icms_value")]
        public float? ValorIcms { get; set; }

        [JsonProperty(PropertyName = "replacement_icms_calculation_basis")]
        public float? BaseCalculoIcmsSubstituicao { get; set; }

        [JsonProperty(PropertyName = "replacement_icms_aliquot")]
        public float? AliquotaIcmsSubstituicao { get; set; }

        [JsonProperty(PropertyName = "replacement_icms_value")]
        public float? ValorIcmsSubstituicao { get; set; }

        [JsonProperty(PropertyName = "ipi_calculation_basis")]
        public float? BaseCalculoIpi { get; set; }

        [JsonProperty(PropertyName = "ipi_aliquot")]
        public float? AliquotaIpi { get; set; }

        [JsonProperty(PropertyName = "ipi_value")]
        public float? ValorIpi { get; set; }

        [JsonProperty(PropertyName = "non_cumulative_pis_value")]
        public float? ValorPisNaoCumulativo { get; set; }

        [JsonProperty(PropertyName = "non_cumulative_cofins_value ")]
        public float? ValorCofinsNaoCumulativo { get; set; }

        [JsonProperty(PropertyName = "total_item_cost")]
        public float CustoTotalItem { get; set; }

        [JsonProperty(PropertyName = "weight")]
        public float? Peso { get; set; }

        [JsonProperty(PropertyName = "concrete_recipe")]
        public string TracoConcreto { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public float Volume { get; set; }

        [JsonProperty(PropertyName = "stock_quantity")]
        public float QuantidadeEstoque { get; set; }

        [JsonProperty(PropertyName = "pis_tax_status_code")]
        public string CodigoSituacaoTributariaPis { get; set; }

        [JsonProperty(PropertyName = "cofins_tax_status_code")]
        public string CodigoSituacaoTributariaCofins { get; set; }

        [JsonProperty(PropertyName = "icms_fiscal_value_1")]
        public float? ValorFicalIcms1 { get; set; }

        [JsonProperty(PropertyName = "icms_fiscal_value_2")]
        public float? ValorFicalIcms2 { get; set; }

        [JsonProperty(PropertyName = "icms_fiscal_value_3")]
        public float? ValorFicalIcms3 { get; set; }

        [JsonProperty(PropertyName = "ipi_fiscal_value_1")]
        public float? ValorFicalIpi1 { get; set; }

        [JsonProperty(PropertyName = "ipi_fiscal_value_2")]
        public float? ValorFicalIpi2 { get; set; }

        [JsonProperty(PropertyName = "ipi_fiscal_value_3")]
        public float? ValorFicalIpi3 { get; set; }

        [JsonProperty(PropertyName = "non_cumulative_pis_percentage")]
        public float? PercentualPisNaoCumulativo { get; set; }

        [JsonProperty(PropertyName = "non_cumulative_cofins_percentage")]
        public float? PercentualCofinsNaoCumulativo { get; set; }

        [JsonProperty(PropertyName = "reference_invoice_supplier")]
        public int? NotaReferenciaFornecedor { get; set; }

        [JsonProperty(PropertyName = "reference_invoice_document_type")]
        public int? NotaReferenciaTipoDocumento { get; set; }

        [JsonProperty(PropertyName = "reference_invoice_series")]
        public string NotaReferenciaSerie { get; set; }

        [JsonProperty(PropertyName = "reference_invoice_number")]
        public long? NotaReferenciaNumero { get; set; }

        [JsonProperty(PropertyName = "reference_invoice_item")]
        public int? NotaReferenciaItem { get; set; }

        [JsonProperty(PropertyName = "pis_calculation_basis")]
        public float? BaseCalculoPis { get; set; }

        [JsonProperty(PropertyName = "pis_percentage")]
        public float? PercentualPis { get; set; }

        [JsonProperty(PropertyName = "pis_value")]
        public float? ValorPis { get; set; }

        [JsonProperty(PropertyName = "cofins_calculation_basis")]
        public float? BaseCalculoCofins { get; set; }

        [JsonProperty(PropertyName = "cofins_percentage")]
        public float? PercentualCofins { get; set; }

        [JsonProperty(PropertyName = "cofins_value")]
        public float? ValorCofins { get; set; }

        [JsonProperty(PropertyName = "stock_intervener")]
        public int? IntervenienteEstoque { get; set; }

        [JsonProperty(PropertyName = "item_pis_tax_status_code")]
        public string ItemPisCodigoSituacaoTributaria { get; set; }

        [JsonProperty(PropertyName = "item_pis_calculation_basis")]
        public float? ItemPisBaseCalculo { get; set; }

        [JsonProperty(PropertyName = "item_pis_percentage")]
        public float? ItemPisPercentual { get; set; }

        [JsonProperty(PropertyName = "item_pis_value")]
        public float? ItemPisValor { get; set; }

        [JsonProperty(PropertyName = "item_cofins_tax_status_code")]
        public string ItemCofinsCodigoSituacaoTributaria { get; set; }

        [JsonProperty(PropertyName = "item_cofins_calculation_basis")]
        public float? ItemCofinsBaseCalculo { get; set; }

        [JsonProperty(PropertyName = "item_cofins_percentage")]
        public float? ItemCofinsPercentual { get; set; }

        [JsonProperty(PropertyName = "item_cofins_value")]
        public float? ItemCofinsValor { get; set; }

        [JsonProperty(PropertyName = "pis_cofins_contribution_taxation_code")]
        public string TributacaoContribuicaoPisCofins { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "initial_validity_of_contribution_taxation")]
        public DateTime? InicioVigenciaTribContribuicao { get; set; }

        [JsonProperty(PropertyName = "finance_operation")]
        public int OperacaoFinanceira { get; set; }

        [JsonProperty(PropertyName = "ipi_tax_status_code")]
        public string IpiCodigoSituacaoTributaria { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public string Unidade { get; set; }

        [JsonProperty(PropertyName = "national_simple_icms_credit_value")]
        public float? ValorCreditoIcmsSimplesNacional { get; set; }

        [JsonProperty(PropertyName = "national_simple_icms_credit_percentage")]
        public float? PercentualCreditoIcmsSimplesNacional { get; set; }

        [JsonProperty(PropertyName = "national_simple_cso")]
        public int? CsoSimplesNacional { get; set; }

        [JsonProperty(PropertyName = "national_simple_icms_calculation_basis")]
        public float? BaseCalculoIcmsSimplesNacional { get; set; }

        [JsonProperty(PropertyName = "item_complement")]
        public NotaFiscalDigitalItemComplementoResponse Complemento { get; set; }
    }
}
