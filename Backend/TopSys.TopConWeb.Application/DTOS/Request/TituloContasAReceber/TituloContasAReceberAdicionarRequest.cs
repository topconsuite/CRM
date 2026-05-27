using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceber
{
    public class TituloContasAReceberAdicionarRequest
    {
        [Required]
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::company" + "::2")]
        [JsonProperty(PropertyName = "company")]
        public int EmpresaCodigo { get; set; } = 0;

        [Required]
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::document_type" + "::2")]
        [JsonProperty(PropertyName = "document_type")]
        public int DocumentoTipoCodigo { get; set; } = 16;
        
        [StringLength(3, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::document_serie" + "::3")]
        [JsonProperty(PropertyName = "document_serie")]
        public string DocumentoSerie { get; set; } = "";

        [Required]
        [Range(1, 99999999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::document_number" + "::11")]
        [JsonProperty(PropertyName = "document_number")]
        public long DocumentoNumero { get; set; } = 0;

        [Required]
        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::sequence" + "::3")]
        [JsonProperty(PropertyName = "sequence")]
        public int? DocumentoSequencia { get; set; }

        [Required]
        [JsonProperty(PropertyName = "splitting")]
        [Range(0, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::splitting" + "::2")]
        public int Desdobramento { get; set; } = 0;

        [Required]
        [JsonProperty(PropertyName = "client")]
        [Range(1, 999999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::client" + "::6")]
        public int IntervenienteCodigo { get; set; } = 0;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "emission_date")]
        public DateTime? DataEmissao { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "operation_date")]
        public DateTime? DataOperacao { get; set; }

        [Required]
        [JsonProperty(PropertyName = "operation_code")]
        [Range(1, 9999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::operation_code" + "::4")]
        public int? OperacaoFinanceiraCodigo { get; set; } = 2;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "due_date")]
        public DateTime? DataVencimento { get; set; }

        [Required]
        [Range(-9999999999.99, 9999999999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS) + "::value" + "::10" + "2")]
        [JsonProperty(PropertyName = "value")]
        public float Valor { get; set; } = 0;

        [JsonProperty(PropertyName = "sum_received")]
        [Range(-9999999999.99, 9999999999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS) + "::sum_received" + "::10" + "2")]
        public float SomaRecebimentos { get; set; } = 0;

        [Required]
        [Range(1, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::cost_center" + "::3")]
        [JsonProperty(PropertyName = "cost_center")]
        public int CentroCustoCodigo { get; set; } = 0;

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::situation" + "::3")]
        [JsonProperty(PropertyName = "situation")]
        public int Situacao { get; set; } = 0;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "situation_date")]
        public DateTime? DataSituacao { get; set; }

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::bill_collector" + "::3")]
        [JsonProperty(PropertyName = "bill_collector")]
        public int BancoPortador { get; set; } = 0;

        [StringLength(100, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::observation" + "::100")]
        [JsonProperty(PropertyName = "observation")]
        public string Observacao { get; set; } = "";

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "liquidation_date")]
        public DateTime? DataLiquidacao { get; set; }

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::operation_liquidate" + "::3")]
        [JsonProperty(PropertyName = "operation_liquidate")]
        public int? OperacaoLiquidacao { get; set; } = 0;

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::fees_liquidation" + "::3")]
        [JsonProperty(PropertyName = "fees_liquidation")]
        public float LiquidacaoJuros { get; set; } = 0;

        [Range(-9999999999.99, 9999999999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS) + "::discount_liquidation" + "::10" + "2")]
        [JsonProperty(PropertyName = "discount_liquidation")]
        public float LiquidacaoDesconto { get; set; } = 0;

        [Range(-9999999999.99, 9999999999.99, ErrorMessage  = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS) + "::expenses_liquidation" + "::10" + "2")]
        [JsonProperty(PropertyName = "expenses_liquidation")]
        public float LiquidacaoDespesas { get; set; } = 0;

        [Range(-9999999999.99, 9999999999.99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_FIELD_MUST_BE_A_NUMBER_WITH_X_DIGITS_AND_Y_DECIMAILS) + "::amount_received_liquidation" + "::10" + "2")]
        [JsonProperty(PropertyName = "amount_received_liquidation")]
        public float LiquidacaoValorRecebido { get; set; } = 0;

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::bank_liquidation" + "::3")]
        [JsonProperty(PropertyName = "bank_liquidation")]
        public int? BancoLiquidacao { get; set; } = 0;
    }
}
