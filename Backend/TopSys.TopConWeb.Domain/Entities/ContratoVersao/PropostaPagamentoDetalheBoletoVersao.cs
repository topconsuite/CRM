using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaPagamentoDetalheBoletoVersao : PropostaPagamentoDetalheVersao, IObraPagamentoDetalheBoleto
    {
        public PropostaPagamentoDetalheBoletoVersao() : base() { }

        public DateTime? DataVencimento { get; set; }

        public DateTime? DataHoraImpressao { get; set; }

        public string NossoNumero { get; set; }

        public string LinhaDigitavel { get; set; }

        public string CodigoDeBarras { get; set; }

        public DateTime? DataRemessa { get; set; }

        public DateTime? DataLiquidacao { get; set; }

        public float ValorLiquidacao { get; set; }

        public string IdLiquidacao { get; set; }

        public override DateTime? DataTitulo()
        {
            if ((DataLiquidacao?.Year ?? 0) < 2000) return DateTime.Today;
            return DataLiquidacao ?? DateTime.Today;
        }

        public override string InfoString()
        {
            return $"NossoNumero:{NossoNumero}|LinhaDigitavel:{LinhaDigitavel}|Dt.Vcto.:{DataVencimento?.ToString("yyyy/MM/dd")}|Valor:{Valor}";
        }
    }
}