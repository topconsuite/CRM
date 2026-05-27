using System;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraPagamentoDetalheCartao : IObraPagamentoDetalhe
    {
        int BandeiraCodigo { get; set; }
        CartaoBandeira Bandeira { get; set; }

        int NumeroCartao { get; set; }

        DateTime DataTransacao { get; set; }

        int QuantidadeParcelas { get; set; }

        string NumeroAutorizacao { get; set; }
    }
}
