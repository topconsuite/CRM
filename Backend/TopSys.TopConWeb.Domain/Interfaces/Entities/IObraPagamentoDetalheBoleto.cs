using System;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraPagamentoDetalheBoleto : IObraPagamentoDetalhe
    {
        DateTime? DataVencimento { get; set; }

        DateTime? DataHoraImpressao { get; set; }

        string NossoNumero { get; set; }

        string LinhaDigitavel { get; set; }

        string CodigoDeBarras { get; set; }

        DateTime? DataRemessa { get; set; }

        DateTime? DataLiquidacao { get; set; }

        float ValorLiquidacao { get; set; }

        string IdLiquidacao { get; set; }
    }
}
