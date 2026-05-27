using System;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraPagamentoDetalheDinheiro : IObraPagamentoDetalhe
    {
        string NumeroRecibo { get; set; }

        DateTime? DataPagamento { get; set; }
    }
}
