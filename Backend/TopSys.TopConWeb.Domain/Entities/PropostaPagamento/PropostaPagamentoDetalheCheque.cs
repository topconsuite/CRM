using System;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaPagamentoDetalheCheque : PropostaPagamentoDetalhe, IObraPagamentoDetalheCheque
    {
        public PropostaPagamentoDetalheCheque() : base() { }

        public int BancoCodigoOficial { get; set; }
        public int BancoAgencia { get; set; }
        public long BancoContaNumero { get; set; }
        public int BancoContaDV { get; set; }
        public int NumeroCheque { get; set; }

        public DateTime? DataRecebimento { get; set; }

        public DateTime? DataBomPara { get; set; }
        public string Observacao { get; set; }

        public override DateTime? DataTitulo()
        {
            if ((DataRecebimento?.Year ?? 0) < 2000) return DateTime.Today;
            return DataRecebimento ?? DateTime.Today;
        }

        public override string InfoString()
        {
            return $"Banco:{BancoCodigoOficial}|NºCheque:{NumeroCheque}|Ag.:{BancoAgencia}|NºC/C:{BancoContaNumero}-{BancoContaDV}|Bom p/:{DataBomPara?.ToString("yyyy/MM/dd")}|Dt.Receb.:{DataRecebimento?.ToString("yyyy/MM/dd")}|Valor:{Valor}";
        }
    }
}
