using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class ObraPagamentoDetalheDepositoDTO : ObraPagamentoDetalheDTO
    {
        public DateTime DataDeposito { get; set; }
        
        public PortadorDTO Portador { get; set; }

        public string NumeroTerminal { get; set; }

        public float Valor { get; set; }

        public string IdAprovacao { get; set; }
    }
}
