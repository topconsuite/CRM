using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class PublicoTituloContasAReceberResponse
    {
        [JsonProperty(PropertyName = "company")]
        public int EmpresaCodigo { get; set; }
        
        [JsonProperty(PropertyName = "document_type")]
        public int DocumentoTipoCodigo { get; set; }
        
        [JsonProperty(PropertyName = "document_serie")]
        public string DocumentoSerie { get; set; }
        
        [JsonProperty(PropertyName = "document_number")]
        public long DocumentoNumero { get; set; }
        
        [JsonProperty(PropertyName = "sequence")]
        public int DocumentoSequencia { get; set; }
        
        [JsonProperty(PropertyName = "bank_brand_code")]
        public int BancoCodigoOficial { get; set; }
        
        [JsonProperty(PropertyName = "agency_number")]
        public int BancoNumeroAgencia { get; set; }
        
        [JsonProperty(PropertyName = "account_number")]
        public int BancoNumeroConta { get; set; }
        
        [JsonProperty(PropertyName = "account_verification_digit")]
        public int BancoDvConta { get; set; }
        
        [JsonProperty(PropertyName = "splitting")]
        public int Desdobramento { get; set; }
        
        [JsonProperty(PropertyName = "client")]
        public int IntervenienteCodigo { get; set; }
        
        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "emission_date")]
        public DateTime? DataEmissao { get; set; }
        
        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "operation_date")]
        public DateTime? DataOperacao { get; set; }
        
        [JsonProperty(PropertyName = "operation_code")]
        public int OperacaoFinanceiraCodigo { get; set; }

        [JsonProperty(PropertyName = "payment_condition_code")]
        public int CondicaoPagamento { get; set; }
        
        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "due_date")]
        public DateTime? DataVencimento { get; set; }
        
        [JsonProperty(PropertyName = "value")]
        public float Valor { get; set; }

        [JsonProperty(PropertyName = "sum_received")]
        public float SomaRecebimentos { get; set; }
        
        [JsonProperty(PropertyName = "cost_center")]
        public int CentroCustoCodigo { get; set; }
        
        [JsonProperty(PropertyName = "situation")]
        public int Situacao { get; set; }
        
        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "situation_date")]
        public DateTime? DataSituacao { get; set; }
        
        [JsonProperty(PropertyName = "bill_collector")]
        public int BancoPortador { get; set; }
        
        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; }
        
        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-dd")]
        [JsonProperty(PropertyName = "liquidation_date")]
        public DateTime? DataLiquidacao { get; set; }
        
        [JsonProperty(PropertyName = "operation_liquidate")]
        public int OperacaoLiquidacao { get; set; }
        
        [JsonProperty(PropertyName = "fees_liquidation")]
        public float LiquidacaoJuros { get; set; }
        
        [JsonProperty(PropertyName = "discount_liquidation")]
        public float LiquidacaoDesconto { get; set; }
        
        [JsonProperty(PropertyName = "expenses_liquidation")]
        public float LiquidacaoDespesas { get; set; }
        
        [JsonProperty(PropertyName = "amount_received_liquidation")]
        public float LiquidacaoValorRecebido { get; set; }
        
        [JsonProperty(PropertyName = "bank_liquidation")]
        public int BancoLiquidacao { get; set; }
        
        [JsonProperty(PropertyName = "cbs_value")]
        public float ValorCbs { get; set; }
        
        [JsonProperty(PropertyName = "ibs_value")]
        public float ValorIbs { get; set; }
        
        [JsonProperty(PropertyName = "is_value")]
        public float ValorIs { get; set; }
        
        [JsonProperty(PropertyName = "include_cbs_ibs_is_to_total")]
        public bool TotalizaCbsIbsIs { get; set; }

        [JsonConverter(typeof(DateTimeToDateJsonConverter), "yyyy-MM-ddTHH:mm:ss")]
        [JsonProperty(PropertyName = "update_date")]
        public DateTime? DataAtualizacao { get; set; }

        [JsonProperty(PropertyName = "billing_type")]
        public TipoDeCobrancaDTO TipoDeCobranca { get; set; }
        
        [JsonProperty(PropertyName = "contract_segmentation")]
        public SegmentacaoDTO Segmentacao { get; set; }
        
        [JsonProperty(PropertyName = "bank_slip_barcode")]
        public string CodigoBarrasBoleto { get; set; }
        
        [JsonProperty(PropertyName = "bank_slip_digitable_line")]
        public string LinhaDigitavelBoleto { get; set; }
        
        [JsonProperty(PropertyName = "bank_slip_our_number")]
        public string NossoNumero { get; set; }
        
        [JsonProperty(PropertyName = "bank_slip_intermediary_our_number")]
        public string NossoNumeroIntermediarioBoleto { get; set; }
        
        [JsonProperty(PropertyName = "consolidated_account_receivable")]
        public ChaveTituloContasAReceberDTO[]  ChaveTitulosDaJuncao{ get; set; }
    }
}
