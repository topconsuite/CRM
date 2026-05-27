using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento
{
    public class CondicoesPagamentoAtualizarRequest
    {
        [Range(1, 9999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::Code" + "::4")]
        [JsonProperty(PropertyName = "code")]
        public int? Codigo { get; set; }

        [MaxLength(50,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::Description" + "::50")]
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [Range(1, 12,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::number_of_payments" + "::12")]
        [JsonProperty(PropertyName = "number_of_payments")]
        public int? QuantidadeParcelas { get; set; }

        [JsonProperty(PropertyName = "percentage_per_payment")]
        public float?[] PercentualParcelas { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::number_of_days_until_due" + "::3")]
        [JsonProperty(PropertyName = "number_of_days_until_due")]
        public int? Vencimento1Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_2nd_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_2nd_payment")]
        public int? Vencimento2Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_3rd_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_3rd_payment")]
        public int? Vencimento3Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_4th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_4th_payment")]
        public int? Vencimento4Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_5th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_5th_payment")]
        public int? Vencimento5Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_6th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_6th_payment")]
        public int? Vencimento6Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_7th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_7th_payment")]
        public int? Vencimento7Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_8th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_8th_payment")]
        public int? Vencimento8Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_9th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_9th_payment")]
        public int? Vencimento9Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_10th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_10th_payment")]
        public int? Vencimento10Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_11th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_11th_payment")]
        public int? Vencimento11Parcela { get; set; }

        [Range(0, 999,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::days_until_the_12th_payment" + "::3")]
        [JsonProperty(PropertyName = "days_until_the_12th_payment")]
        public int? Vencimento12Parcela { get; set; }

        [MaxLength(1,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::Condition" + "::1")]
        [JsonProperty(PropertyName = "condition")]
        public string CondicaoDaCobrancaCod { get; set; }

        [JsonProperty(PropertyName = "fixed_due_date_in_the_week")]
        public bool? VencimentoFixoSemana { get; set; }

        [JsonProperty(PropertyName = "day_of_the_week")]
        public int? DiaVencimentoFixoSemana { get; set; }

        [Range(1, 3,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_LESS_THAN_ZERO_MORE_DIGITS_THAN_ALLOWED) +
                           "::business_day_due_date" + "::3")]
        [JsonProperty(PropertyName = "business_day_due_date")]
        public int? DiaUltimoVencimento { get; set; }

        [MaxLength(10,
            ErrorMessage = nameof(EResourcesDefaultMessages.DEFAULT_MESSAGES_MORE_CHARACTERS_THAN_ALLOWED) +
                           "::external_id" + "::10")]
        [JsonProperty(PropertyName = "external_id")]
        public string IdExterno { get; set; }

        [JsonProperty(PropertyName = "billing_type")]
        public int?[] TiposDeCobrancaCodigos { get; set; }
    }
}