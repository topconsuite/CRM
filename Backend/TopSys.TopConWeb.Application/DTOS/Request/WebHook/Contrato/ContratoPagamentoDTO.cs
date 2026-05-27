using Newtonsoft.Json;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Request.WebHook
{

    public class ContratoPagamentoDTO
    {
        public ContratoPagamentoDTO(int sequencia, string forma, int codigoTipoDeCobranca, float valor, CondicaoPagamentoDTO condicaoPagamento)
        {
            Sequencia = sequencia;
            Forma = forma;
            CodigoTipoDeCobranca = codigoTipoDeCobranca;
            Valor = valor;
            CondicaoPagamento = condicaoPagamento;
        }

        [JsonProperty("sequence")]
        public int Sequencia { get; set; }

        [JsonProperty("method")]
        public string Forma { get; set; }

        [JsonProperty("billing_code")]
        public int CodigoTipoDeCobranca { get; set; }

        [JsonProperty("value")]
        public float Valor { get; set; }

        [JsonProperty("payment_condition")]
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }

        [JsonProperty("details")]
        public IEnumerable<ContratoPagamentoDetalheDTO> Detalhes { get; set; }

    }

}
