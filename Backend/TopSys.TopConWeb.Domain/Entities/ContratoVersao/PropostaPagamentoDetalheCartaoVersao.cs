using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class PropostaPagamentoDetalheCartaoVersao : PropostaPagamentoDetalheVersao, IObraPagamentoDetalheCartao
    {
        public PropostaPagamentoDetalheCartaoVersao() : base() { }

        public int BandeiraCodigo { get; set; }
        public CartaoBandeira Bandeira { get; set; }

        public int NumeroCartao { get; set; }

        public DateTime DataTransacao { get; set; }

        public int QuantidadeParcelas { get; set; }

        public string NumeroAutorizacao { get; set; }

        public override DateTime? DataTitulo()
        {
            return DataTransacao;
        }

        public override string InfoString()
        {
            return $"Bandeira:{BandeiraCodigo}|NºCartão:{NumeroCartao}|Dt.Trans.:{DataTransacao.ToString("yyyy/MM/dd")}|NºAutoriz.:{NumeroAutorizacao}|No.Parcelas:{QuantidadeParcelas}|Valor:{Valor}";
        }
    }
}
