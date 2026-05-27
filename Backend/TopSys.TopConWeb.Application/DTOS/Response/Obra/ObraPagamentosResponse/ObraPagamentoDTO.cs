using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class ObraPagamentoDTO
    {
        public int Sequencia { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public TipoCobrancaDTO TipoCobranca { get; set; }
        public float Valor;
        public ICollection<ObraPagamentoDetalheDTO> Detalhes { get; set; }

        public bool NecessitaAprovacao { get; set; }
        public string IdAprovacao { get; set; }

        public int StatusAprovacao { get; set; }
        public string AtivoSimNao { get; set; }
    }
}
