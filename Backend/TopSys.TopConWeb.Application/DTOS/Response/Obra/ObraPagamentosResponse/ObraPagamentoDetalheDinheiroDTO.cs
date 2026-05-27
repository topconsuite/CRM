using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPagamentosResponse
{
    public class ObraPagamentoDetalheDinheiroDTO : ObraPagamentoDetalheDTO
    {
        public string NumeroRecibo { get; set; }

        public DateTime? DataPagamento { get; set; }

        public float Valor { get; set; }
    }
}
