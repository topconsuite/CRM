using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoPagamentoDetalheDinheiroVersao : ContratoPagamentoDetalheVersao, IObraPagamentoDetalheDinheiro
    {
        public ContratoPagamentoDetalheDinheiroVersao() : base() { }

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
