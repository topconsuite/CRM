using Newtonsoft.Json;
using System;
using TopSys.TopConWeb.Application.DTOS.Response.Fatura;
using TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber;
using Topsys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital
{
    public class NotaFiscalDigitalResponse
    {
        [JsonProperty(PropertyName = "branch")]
        public int Filial { get; set; }

        [JsonProperty(PropertyName = "client")]
        public int Cliente { get; set; }

        [JsonProperty(PropertyName = "client_contract")]
        public int ClienteContrato { get; set; }
        
        [JsonProperty(PropertyName = "client_cnpj_cpf")]
        public string ClienteCfpCnpj { get; set; }
        
        [JsonProperty(PropertyName = "client_state_registration")]
        public string ClienteInscEstadual { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public int TipoDocumento { get; set; }

        [JsonProperty(PropertyName = "series")]
        public string Serie { get; set; }

        [JsonProperty(PropertyName = "invoice_number")]
        public long Numero { get; set; }

        [JsonProperty(PropertyName = "invoice_sequence")]
        public int Sequencia { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "invoice_date")]
        public DateTime DataNf { get; set; }
        
        [JsonProperty(PropertyName = "concrete_batching_plant_external_id")]
        public string UsinaExternalId { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "operation_date")]
        public DateTime DataOperacao { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "issue_date")]
        public DateTime? DataEmissao { get; set; }

        [JsonProperty(PropertyName = "issue_time")]
        public int HoraEmissao { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "cancellation_release_date")]
        public DateTime? DataLancamentoCancelamento { get; set; }

        [JsonProperty(PropertyName = "canceled")]
        public bool Cancelada { get; set; }

        [JsonProperty(PropertyName = "operation_indicator")]
        public int IndicadorOperacao { get; set; }

        [JsonProperty(PropertyName = "issuer_indicator")]
        public int IndicadorEmitente { get; set; }

        [JsonProperty(PropertyName = "operation_transaction")]
        public int TransacaoDaOperacao { get; set; }

        [JsonProperty(PropertyName = "fiscal_document_model")]
        public string ModeloDocumentoFiscalSefaz { get; set; }

        [JsonProperty(PropertyName = "fiscal_situation")]
        public int? SituacaoFiscal { get; set; }

        [JsonProperty(PropertyName = "cfop")]
        public int Cfop { get; set; }

        [JsonProperty(PropertyName = "cfop_sequence")]
        public int? SequenciaCfop { get; set; }

        [JsonProperty(PropertyName = "seller")]
        public int? Vendedor { get; set; }

        [JsonProperty(PropertyName = "destination_uf")]
        public string UfDestino { get; set; }

        [JsonProperty(PropertyName = "products_value")]
        public float? ValorMercadoria { get; set; }

        [JsonProperty(PropertyName = "discount_value")]
        public float? ValorDesconto { get; set; }

        [JsonProperty(PropertyName = "freight_value")]
        public float? ValorFrete { get; set; }

        [JsonProperty(PropertyName = "insurance_value")]
        public float? ValorSeguro { get; set; }

        [JsonProperty(PropertyName = "other_expenses_value")]
        public float? ValorOutrasDespesas { get; set; }

        [JsonProperty(PropertyName = "accounting_value")]
        public float? ValorContabil { get; set; }

        [JsonProperty(PropertyName = "icms_calculation_basis")]
        public float? BaseCalculoIcms { get; set; }

        [JsonProperty(PropertyName = "icms_aliquot")]
        public float? AliquotaIcms { get; set; }

        [JsonProperty(PropertyName = "icms_value")]
        public float? ValorIcms { get; set; }

        [JsonProperty(PropertyName = "replacement_icms_calculation_basis")]
        public float? BaseCalculoIcmsSubstituicao { get; set; }

        [JsonProperty(PropertyName = "replacement_icms_value")]
        public float? ValorIcmsSubstituicao { get; set; }

        [JsonProperty(PropertyName = "ipi_value")]
        public float? ValorIpi { get; set; }

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

        [JsonProperty(PropertyName = "transporter")]
        public int? Transportador { get; set; }

        [JsonProperty(PropertyName = "volume_quantity")]
        public float? QuantidadeVolume { get; set; }

        [JsonProperty(PropertyName = "volume_species")]
        public string EspecieVolume { get; set; }

        [JsonProperty(PropertyName = "gross_weight")]
        public float? PesoBruto { get; set; }

        [JsonProperty(PropertyName = "net_weight")]
        public float? PesoLiquido { get; set; }

        [JsonProperty(PropertyName = "freight_indicator")]
        public int? IndicadorFrete { get; set; }

        [JsonProperty(PropertyName = "vehicle_identification")]
        public string IdentificacaoVeiculoPlaca { get; set; }

        [JsonProperty(PropertyName = "fiscal_observation")]
        public string ObservacaoFiscal { get; set; }

        [JsonProperty(PropertyName = "destination_branch")]
        public int? FilialDestino { get; set; }

        [JsonProperty(PropertyName = "vehicle_uf")]
        public string UfVeiculo { get; set; }

        [JsonProperty(PropertyName = "pis_value")]
        public float? ValorPis { get; set; }

        [JsonProperty(PropertyName = "cofins_value")]
        public float? ValorCofins { get; set; }

        [JsonProperty(PropertyName = "fiscal_message")]
        public string MensagemFiscalNfe { get; set; }

        [JsonProperty(PropertyName = "iss_supplier")]
        public int? FornecedorIss { get; set; }

        [JsonProperty(PropertyName = "pis_total_base")]
        public float? TotalBasePis { get; set; }

        [JsonProperty(PropertyName = "pis_total")]
        public float? TotalPis { get; set; }

        [JsonProperty(PropertyName = "cofins_total_base")]
        public float? TotalBaseCofins { get; set; }

        [JsonProperty(PropertyName = "cofins_total")]
        public float? TotalCofins { get; set; }

        [JsonProperty(PropertyName = "invoice_key")]
        public string ChaveNfe { get; set; }
        
        [JsonProperty(PropertyName = "return_invoice_key")]
        public string ChaveNfDevolucao { get; set; }

        [JsonProperty(PropertyName = "operation")]
        public int? Operacao { get; set; }

        [JsonProperty(PropertyName = "service_value")]
        public float? ValorServico { get; set; }

        [JsonProperty(PropertyName = "cost_center")]
        public int? CentroCusto { get; set; }

        [JsonProperty(PropertyName = "requester")]
        public int? Requisitante { get; set; }

        [JsonProperty(PropertyName = "energy_consumption_classification")]
        public int? ClassificacaoConsumoEnergia { get; set; }

        [JsonProperty(PropertyName = "connection_type")]
        public int? TipoLigacao { get; set; }

        [JsonProperty(PropertyName = "energy_tension_group")]
        public int? GrupoTensaoEnregia { get; set; }

        [JsonProperty(PropertyName = "subscriber_type")]
        public int? TipoAssinante { get; set; }

        [JsonProperty(PropertyName = "freight_type")]
        public int? TipoFrete { get; set; }

        [JsonProperty(PropertyName = "freight_nature")]
        public int? NaturezaFrete { get; set; }

        [JsonProperty(PropertyName = "stock_branch")]
        public int? FilialEstoque { get; set; }

        [JsonProperty(PropertyName = "request_number")]
        public int? NumeroRequisicao { get; set; }

        [JsonProperty(PropertyName = "request_year")]
        public int? AnoRequisicao { get; set; }

        [JsonProperty(PropertyName = "request_company")]
        public int? EmpresaRequisicao { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "update_date")]
        public DateTime? DataAtualizacao { get; set; }
        
        [JsonProperty(PropertyName = "parent_service_invoice")]
        public ChaveFaturaDTO ChaveNotaServicoPai { get; set; }
        
        [JsonProperty(PropertyName = "account_receivable")]
        public ChaveTituloContasAReceberDTO ChaveTituloContasAReceber { get; set; }

        [JsonProperty(PropertyName = "digital_invoice_complement")]
        public NotaFiscalDigitalComplementoResponse Complemento { get; set; }

        [JsonProperty(PropertyName = "tax_authority_details")]
        public NotaFiscalDigitalDetalhesFiscaisResponse DetalhesFiscais { get; set; }

        [JsonProperty(PropertyName = "tax_distribution_details")]
        public NotaFiscalDigitalDetalhesDistribuicaoResponse DetalhesDistribuicao { get; set; }

        [JsonProperty(PropertyName = "items")]
        public NotaFiscalDigitalItemResponse[] Itens { get; set; }
    }
}
