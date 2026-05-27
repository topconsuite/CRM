using Newtonsoft.Json;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoIntegracao
{
    public class ContratoPagamentoSimplificadoResponse
    {

        [JsonProperty("Sequence")]
        public int Sequencia { get; set; }

        [JsonProperty("Method")]
        public string Forma { get; set; }

        [JsonProperty("Payment_condition")]
        public CondicaoPagamentoSimplificadoResponse CondicaoPagamento { get; set; }

        [JsonProperty("value")]
        public float Valor { get; set; }

        [JsonProperty("Details")]
        public IEnumerable<ContratoPagamentoDetalheSimplificadoResponse> Detalhes { get; set; }

    }

    public class CondicaoPagamentoSimplificadoResponse
    {

        [JsonProperty("Code")]
        public int Codigo { get; set; }

        [JsonProperty("Description")]
        public string Descricao { get; set; }

    }

}

