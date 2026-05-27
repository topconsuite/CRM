using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.SituacaoFinanceira
{
    public class SituacaoFinanceiraAtualizarRequest
    {
        [Range(1, 99, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) + "::Code" + "::2")]
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }

        [MaxLength(30, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) + "::Description" + "::30")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; } 

        [JsonProperty(PropertyName = "fix")]
        public bool? Fixo { get; set; }

        [Range(0, 999, ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_DIGITS_THAN_ALLOWED) + "::Sell_Operation" + "::3")]
        [JsonProperty(PropertyName = "sell_operation")]
        public int? OperacaoBaixa { get; set; }
    }
}
