using System;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoPagamentoDetalheDeposito : ContratoPagamentoDetalhe, IObraPagamentoDetalheDeposito
    {
        public ContratoPagamentoDetalheDeposito() : base() { }

        public DateTime DataDeposito { get; set; }

        public int? PortadorCodigo { get; set; } = 0;
        public Portador Portador { get; set; }

        public int TomadorBanco { get; set; }
        public string TomadorAgencia { get; set; }
        public string TomadorNumeroConta { get; set; }

        public string NumeroTerminal { get; set; }

        public string IdAprovacao { get; set; }

        public override DateTime? DataTitulo()
        {
            return DataDeposito;
        }

        public override string InfoString()
        {
            return $"Dt.Depósito:{DataDeposito.ToString("yyyy/MM/dd")}|Portador:{PortadorCodigo}|NºTerminal:{NumeroTerminal}|Valor:{Valor}";
        }
    }
}
