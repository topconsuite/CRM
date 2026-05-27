using System;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraPagamentoDetalheCheque : IObraPagamentoDetalhe
    {
        int BancoCodigoOficial { get; set; }
        int BancoAgencia { get; set; }
        long BancoContaNumero { get; set; }
        int BancoContaDV { get; set; }
        int NumeroCheque { get; set; }

        DateTime? DataRecebimento { get; set; }

        DateTime? DataBomPara { get; set; }
        string Observacao { get; set; }
    }
}
