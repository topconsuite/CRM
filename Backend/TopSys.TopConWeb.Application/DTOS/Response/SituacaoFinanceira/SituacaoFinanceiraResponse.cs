using Newtonsoft.Json;

namespace TopSys.TopConWeb.Application.DTOS.Response.SituacaoFinanceira
{
    public class SituacaoFinanceiraResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "fix")]
        public bool Fixo { get; set; }

        [JsonProperty(PropertyName = "sell_operation")]
        public int OperacaoBaixa { get; set; }
    }
}
