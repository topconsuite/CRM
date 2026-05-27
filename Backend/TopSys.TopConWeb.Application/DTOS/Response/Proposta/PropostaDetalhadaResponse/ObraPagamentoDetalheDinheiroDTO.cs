using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class ObraPagamentoDetalheDinheiroDTO : ObraPagamentoDetalheDTO
    {
        public string NumeroRecibo { get; set; }

        public DateTime? DataPagamento { get; set; }
    }
}
