using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.OperacaoFinanceira
{
    public class OperacaoFinanceiraAtualizarRequest
    {
        [Range(1, 9999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::Code" + "::4")]
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }

        [MaxLength(30,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::Description" + "::30")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [MaxLength(2,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::subsystem" + "::2")]
        [JsonProperty(PropertyName = "subsystem")]
        public string SubSistema { get; set; }
        
        [JsonProperty(PropertyName = "inclusion_discharge")]
        public string InclusaoOuBaixa { get; set; }

        [JsonProperty(PropertyName = "no_finance_activity")]
        public bool? SemMovFinanceiro { get; set; }

        [JsonProperty(PropertyName = "update_bank")]
        public int? AtualizaBanco { get; set; }

        [Range(0, 9999, ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
            "::sell_operation" + "::4")]
        [JsonProperty(PropertyName = "sell_operation")]
        public int? OperacaoBaixa { get; set; }

        [Range(0, 9999, ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
            "::bank_movement_operation" + "::4")]
        [JsonProperty(PropertyName = "bank_movement_operation")]
        public int? OperacaoMovBco { get; set; }

        [JsonProperty(PropertyName = "revenue_expense")]
        public int? ReceitaDespesa { get; set; }

        [JsonProperty(PropertyName = "cost_center_that_use")]
        public int[] CentrosDeCusto { get; set; }
    }
}