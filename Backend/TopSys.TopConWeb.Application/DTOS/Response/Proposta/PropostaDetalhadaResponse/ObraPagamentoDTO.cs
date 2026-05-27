using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
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

        public float ValorApropriado { get; set; }
        public string AtivoSimNao { get; set; }
    }
}
