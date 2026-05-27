using System;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraPagamentoDetalheDeposito : IObraPagamentoDetalhe
    {
        DateTime DataDeposito { get; set; }

        int? PortadorCodigo { get; set; }
        Portador Portador { get; set; }

        int TomadorBanco { get; set; }
        string TomadorAgencia { get; set; }
        string TomadorNumeroConta { get; set; }

        string NumeroTerminal { get; set; }

        string IdAprovacao { get; set; }
    }
}
