using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento
{
    public class CondicaoDePagamentoResponse
    {

        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }
        
        [JsonProperty(PropertyName = "number_of_payments")]
        public int QuantidadeParcelas { get; set; }
        
        [JsonProperty(PropertyName = "percentage_per_payment")]
        public float[] PercentualParcelas { get; set; }
        
        [JsonProperty(PropertyName = "number_of_days_until_due")]
        public int Vencimento1Parcela { get; set; }

        [JsonProperty(PropertyName = "days_until_the_2nd_payment")]
        public int Vencimento2Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_3rd_payment")]
        public int Vencimento3Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_4th_payment")]
        public int Vencimento4Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_5th_payment")]
        public int Vencimento5Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_6th_payment")]
        public int Vencimento6Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_7th_payment")]
        public int Vencimento7Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_8th_payment")]
        public int Vencimento8Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_9th_payment")]
        public int Vencimento9Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_10th_payment")]
        public int Vencimento10Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_11th_payment")]
        public int Vencimento11Parcela { get; set; }
        
        [JsonProperty(PropertyName = "days_until_the_12th_payment")]
        public int Vencimento12Parcela { get; set; }
        
        [JsonProperty(PropertyName = "condition")]
        public char CondicaoDaCobrancaCod { get; set; }
        
        [JsonProperty(PropertyName = "fixed_due_date_in_the_week")]
        public bool VencimentoFixoSemana { get; set; }
        
        [JsonProperty(PropertyName = "day_of_the_week")]
        public int DiaVencimentoFixoSemana { get; set; }
        
        [JsonProperty(PropertyName = "business_day_due_date")]
        public int DiaUltimoVencimento { get; set; }
        
        [JsonProperty(PropertyName = "external_id")]
        public string IdExterno { get; set; }
        
        [JsonProperty(PropertyName = "billing_type")]
        public int[] TiposDeCobrancaCodigos { get; set; }
    }
}