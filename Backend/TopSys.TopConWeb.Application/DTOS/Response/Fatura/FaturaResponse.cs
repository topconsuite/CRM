using Newtonsoft.Json;
using System;
using TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.Fatura
{
    public class FaturaResponse
    {
        [JsonProperty(PropertyName = "branch")]
        public int Filial { get; set; }

        [JsonProperty(PropertyName = "client")]
        public int Cliente { get; set; }

        [JsonProperty(PropertyName = "client_external_id")]
        public string ClienteCodigoExterno { get; set; }

        [JsonProperty(PropertyName = "client_cnpj_cpf")]
        public string ClienteCfpCnpj { get; set; }
        
        [JsonProperty(PropertyName = "client_state_registration")]
        public string ClienteInscEstadual { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumento { get; set; }

        [JsonProperty(PropertyName = "invoice_number")]
        public long Numero { get; set; }

        [JsonProperty(PropertyName = "invoice_series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "invoice_sub_series")]
        public int SubSerie { get; set; }

        [JsonProperty(PropertyName = "rps_number")]
        public int NumeroRps { get; set; }

        [JsonProperty(PropertyName = "service_invoice_number")]
        public long NumeroNfse { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "invoice_date")]
        public DateTime DataNf { get; set; }

        [JsonProperty(PropertyName = "service_provider_fiscal_code")]
        public int CodFiscalPrestadorServico { get; set; }

        [JsonProperty(PropertyName = "municipality_service_provision")]
        public int MunicipioPrestacaoServico { get; set; }

        [JsonProperty(PropertyName = "provision_nature")]
        public int NaturezaPrestacao { get; set; }

        [JsonProperty(PropertyName = "contract_concrete_batching_plant")]
        public int ContratoUsina { get; set; }

        [JsonProperty(PropertyName = "contract_number")]
        public int ContratoNumero { get; set; }

        [JsonProperty(PropertyName = "contract_year")]
        public int ContratoAno { get; set; }

        [JsonProperty(PropertyName = "invoice_in_care")]
        public string FaturamentoAC { get; set; }

        [JsonProperty(PropertyName = "invoicing_location")]
        public int LocalFaturamento { get; set; }

        [JsonProperty(PropertyName = "billing_location")]
        public int LocalCobranca { get; set; }

        [JsonProperty(PropertyName = "payment_condition")]
        public int CondicaoPagamento { get; set; }

        [JsonProperty(PropertyName = "payment_indicator")]
        public int IndicadorPagamento { get; set; }

        [JsonProperty(PropertyName = "canceled")]
        public bool Cancelada { get; set; }

        [JsonProperty(PropertyName = "cancellation_reason")]
        public int MotivoCancelamento { get; set; }

        [JsonProperty(PropertyName = "gross_service_value")]
        public float ValorServicoBruto { get; set; }

        [JsonProperty(PropertyName = "discount_value")]
        public float ValorDesconto { get; set; }

        [JsonProperty(PropertyName = "service_value")]
        public float ValorServico { get; set; }

        [JsonProperty(PropertyName = "own_materials_used_value")]
        public float ValorMateriaisProprio { get; set; }

        [JsonProperty(PropertyName = "third_party_materials_used_value")]
        public float ValorMateriaisTerceiros { get; set; }

        [JsonProperty(PropertyName = "incidental_expenses")]
        public float ValorDespesasAcessorias { get; set; }

        [JsonProperty(PropertyName = "subcontracted_value")]
        public float ValorSubContratada { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public float ValorTotal { get; set; }

        [JsonProperty(PropertyName = "iss_calculation_basis")]
        public float BaseCalculoIss { get; set; }

        [JsonProperty(PropertyName = "iss_percentage")]
        public float PercentualIss { get; set; }

        [JsonProperty(PropertyName = "iss_value")]
        public float ValorIss { get; set; }

        [JsonProperty(PropertyName = "invoice_fiscal_observation")]
        public string ObservacaoFiscalNf { get; set; }

        [JsonProperty(PropertyName = "retention_calculation_basis")]
        public float BaseCalculoRetencao { get; set; }

        [JsonProperty(PropertyName = "withheld_iss_percentage")]
        public float PercentualIssRetido { get; set; }

        [JsonProperty(PropertyName = "withheld_iss_value")]
        public float ValorIssRetido { get; set; }

        [JsonProperty(PropertyName = "irrf_calculation_basis")]
        public float BaseCalculoIrrf { get; set; }

        [JsonProperty(PropertyName = "irrf_percentage")]
        public float PercentualIrrf { get; set; }

        [JsonProperty(PropertyName = "irrf_value")]
        public float ValorIrrf { get; set; }

        [JsonProperty(PropertyName = "pis_percentage")]
        public float PercentualPis { get; set; }

        [JsonProperty(PropertyName = "pis_value")]
        public float ValorPis { get; set; }

        [JsonProperty(PropertyName = "cofins_percentage")]
        public float PercentualCofins { get; set; }

        [JsonProperty(PropertyName = "cofins_value")]
        public float ValorCofins { get; set; }

        [JsonProperty(PropertyName = "withheld_inss_calculation_basis")]
        public float BaseCalculoInssRetido { get; set; }

        [JsonProperty(PropertyName = "withheld_inss_value")]
        public float ValorInssRetido { get; set; }

        [JsonProperty(PropertyName = "representative")]
        public int Representante { get; set; }

        [JsonProperty(PropertyName = "representative_commission_value")]
        public float ValorComissaoRepresentante { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public int Vendedor { get; set; }

        [JsonProperty(PropertyName = "seller_commission_value")]
        public float ValorComissaoVendedor { get; set; }

        [JsonProperty(PropertyName = "quantity_of_installments")]
        public int QuantidadeParcelas { get; set; }

        [JsonProperty(PropertyName = "billing_number")]
        public long NumeroFaturamento { get; set; }

        [JsonProperty(PropertyName = "pump_total_value")]
        public float? ValorTotalBomba { get; set; }

        [JsonProperty(PropertyName = "iss_pump_calculation_basis")]
        public float? BaseCalculoIssBomba { get; set; }

        [JsonProperty(PropertyName = "pump_iss_value")]
        public float? ValorIssBomba { get; set; }

        [JsonProperty(PropertyName = "pump_withheld_iss_value")]
        public float? ValorIssRetidoBomba { get; set; }

        [JsonProperty(PropertyName = "withheld_inss_percentage")]
        public float PercentualInssRetido { get; set; }

        [JsonProperty(PropertyName = "pis_calculation_basis")]
        public float BaseCalculoPis { get; set; }

        [JsonProperty(PropertyName = "cofins_calculation_basis")]
        public float BaseCalculoCofins { get; set; }

        [JsonProperty(PropertyName = "irpj_calculation_basis")]
        public float BaseCalculoIrpj { get; set; }

        [JsonProperty(PropertyName = "irpj_percentage")]
        public float PercentualIrpj { get; set; }

        [JsonProperty(PropertyName = "irpj_value")]
        public float ValorIrpj { get; set; }

        [JsonProperty(PropertyName = "csll_calculation_basis")]
        public float BaseCalculoCsll { get; set; }

        [JsonProperty(PropertyName = "csll_percentage")]
        public float PercentualCsll { get; set; }

        [JsonProperty(PropertyName = "csll_value")]
        public float ValorCsll { get; set; }

        [JsonProperty(PropertyName = "service_invoice_verification_code")]
        public string CodigoVerificadorNfse { get; set; }

        [JsonProperty(PropertyName = "internal_request")]
        public string RequisicaoInterna { get; set; }

        [JsonProperty(PropertyName = "requester")]
        public int Requisitante { get; set; }

        [JsonProperty(PropertyName = "iss_supplier")]
        public int FornecedorIss { get; set; }

        [JsonProperty(PropertyName = "pis_base_total")]
        public float TotalBasePis { get; set; }

        [JsonProperty(PropertyName = "pis_total")]
        public float TotalPis { get; set; }

        [JsonProperty(PropertyName = "cofins_base_total")]
        public float TotalBaseCofins { get; set; }

        [JsonProperty(PropertyName = "cofins_total")]
        public float TotalCofins { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "release_date")]
        public DateTime DataLancamento { get; set; }

        [JsonProperty(PropertyName = "pending")]
        public int Pendente { get; set; }

        [JsonProperty(PropertyName = "withheld_taxes_calculation_basis")]
        public float BcRetencoes { get; set; }

        [JsonProperty(PropertyName = "cost_center")]
        public int CentroCusto { get; set; }

        [JsonProperty(PropertyName = "receipt_number")]
        public long NumeroRecibo { get; set; }

        [JsonProperty(PropertyName = "encapsulation")]
        public long Encapsulamento { get; set; }

        [JsonProperty(PropertyName = "protocol_number")]
        public string NumeroProtocolo { get; set; }

        [JsonProperty(PropertyName = "inss_percentage")]
        public float PercentualInss { get; set; }

        [JsonProperty(PropertyName = "inss_value")]
        public float ValorInss { get; set; }

        [JsonProperty(PropertyName = "inss_calculation_basis")]
        public float BaseCalculoInss { get; set; }

        [JsonProperty(PropertyName = "contract_segmentation")]
        public int SegmentacaoId { get; set; }
        
        [JsonProperty(PropertyName = "contract_segmentation_name")]
        public string SegmentacaoNome { get; set; }
        
        [JsonProperty(PropertyName = "contract_segmentation_shortname")]
        public string SegmentacaoNomeAbreviado { get; set; }
        
        [JsonProperty(PropertyName = "construction_cno_code")]
        public string CnoObra { get; set; }
        
        [JsonProperty(PropertyName = "construction_site_name")]
        public string NomeObra { get; set; }
        
        [JsonProperty(PropertyName = "construction_complete_address")]
        public string EnderecoObraCompleto { get; set; }
        
        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "update_date")]
        public DateTime? DataAtualizacao { get; set; }
        
        [JsonProperty(PropertyName = "linked_digital_invoice")]
        public ChaveNotaFiscalDTO ChaveNotaVendaMateriais { get; set; }
        
        [JsonProperty(PropertyName = "parent_service_invoice")]
        public ChaveFaturaDTO ChaveNotaServicoPai { get; set; }
        
        [JsonProperty(PropertyName = "linked_account_receivable")]
        public ChaveTituloContasAReceberDTO ChaveTituloJuncao { get; set; }
        
        [JsonProperty(PropertyName = "base_cbs_ibs")]
        public decimal BaseCbsIbs{ get; set; }
        
        [JsonProperty(PropertyName = "value_cbs")]
        public decimal ValorCbs { get; set; }
        
        [JsonProperty(PropertyName = "value_ibs_municipal")]
        public decimal ValorIbsMunicipal { get; set; }
        
        [JsonProperty(PropertyName = "value_ibs_state")]
        public decimal ValorIbsEstadual { get; set; }

        [JsonProperty(PropertyName = "value_ibs")]
        public decimal ValorIbs { get; set; }

        [JsonProperty(PropertyName = "items")]
        public FaturaItemResponse[] Itens { get; set; }
    }
}
