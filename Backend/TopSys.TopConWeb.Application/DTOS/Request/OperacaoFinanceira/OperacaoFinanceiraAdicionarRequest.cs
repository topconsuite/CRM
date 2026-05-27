using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.OperacaoFinanceira
{
    public class OperacaoFinanceiraAdicionarRequest
    {
        [Required(ErrorMessage = "The code is required")]
        [Range(1, 9999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::Code" + "::4")]
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::Description")]
        [MaxLength(30,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::Description" + "::30")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::subsystem")]
        [MaxLength(2,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::subsystem" + "::2")]
        [JsonProperty(PropertyName = "subsystem")]
        public string SubSistema { get; set; }

        [Required(ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::inclusion_discharge")]
        [MaxLength(1, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                                     "::inclusion_discharge" + "::1")]
        [JsonProperty(PropertyName = "inclusion_discharge")]
        public string InclusaoOuBaixa { get; set; }

        [JsonProperty(PropertyName = "no_finance_activity")]
        public bool? SemMovFinanceiro { get; set; }

        [Required(ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::update_bank")]
        [JsonProperty(PropertyName = "update_bank")]
        public int AtualizaBanco { get; set; }

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

        [Required(ErrorMessage =
            nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY) + "::cost_center_that_use")]
        [JsonProperty(PropertyName = "cost_center_that_use")]
        public int[] CentrosDeCusto { get; set; }
    }
}