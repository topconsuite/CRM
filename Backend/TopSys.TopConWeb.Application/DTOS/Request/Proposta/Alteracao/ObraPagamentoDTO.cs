using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class ObraPagamentoDTO
    {
        public int Sequencia { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public TipoCobrancaDTO TipoCobranca { get; set; }
        public float Valor;
        public ICollection<ObraPagamentoDetalheDTO> Detalhes { get; set; }
        public string AtivoSimNao { get; set; }
    }
}
