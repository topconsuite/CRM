using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.OperacaoFinanceira
{
    public class OperacaoFinanceiraResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }
        
        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }
        
        [JsonProperty(PropertyName = "subsystem")]
        public string SubSistema { get; set; }
        
        [JsonProperty(PropertyName = "inclusion_discharge")]
        public string InclusaoOuBaixa { get; set; }
        
        [JsonProperty(PropertyName = "no_finance_activity")]
        public bool SemMovFinanceiro { get; set; }
        
        [JsonProperty(PropertyName = "update_bank")]
        public int AtualizaBanco { get; set; }
        
        [JsonProperty(PropertyName = "sell_operation")]
        public int OperacaoBaixa { get; set; }
        
        [JsonProperty(PropertyName = "bank_movement_operation")]
        public int OperacaoMovBco { get; set; }
        
        [JsonProperty(PropertyName = "revenue_expense")]
        public int ReceitaDespesa { get; set; }

        [JsonProperty(PropertyName = "cost_center_that_use")]
        public int[] CentrosDeCusto { get; set; }
    }
}