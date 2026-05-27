using System;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoPagamentoDetalheDinheiro : ContratoPagamentoDetalhe, IObraPagamentoDetalheDinheiro
    {
        public ContratoPagamentoDetalheDinheiro() : base() { }

        public string NumeroRecibo { get; set; }

        public DateTime? DataPagamento { get; set; }

        public override DateTime? DataTitulo()
        {
            if ((DataPagamento?.Year ?? 0) < 2000) return DateTime.Today;
            return DataPagamento ?? DateTime.Today;
        }

        public override string InfoString()
        {
            return $"NºRecibo.:{NumeroRecibo}|Dt.Pagamento.:{DataPagamento?.ToString("yyyy/MM/dd")}|Valor:{Valor}";
        }
    }
}
