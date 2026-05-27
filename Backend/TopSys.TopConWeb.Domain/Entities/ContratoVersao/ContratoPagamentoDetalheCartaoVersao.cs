using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ContratoPagamentoDetalheCartaoVersao : ContratoPagamentoDetalheVersao, IObraPagamentoDetalheCartao
    {
        public ContratoPagamentoDetalheCartaoVersao() : base() { }

        public int BandeiraCodigo { get; set; }
        public CartaoBandeira Bandeira { get; set; }

        public int NumeroCartao { get; set; }

        public string NumeroCartaoAsString
        {
            get
            {
                return NumeroCartao.ToString().PadLeft(4, '0');
            }
        }

        public DateTime DataTransacao { get; set; }

        public int QuantidadeParcelas { get; set; }

        public string NumeroAutorizacao { get; set; }

        public float ValorPorParcela
        {
            get { return (float)Math.Round(Valor / QuantidadeParcelas, 2); }
        }

        public IList<float> ValoresParcelas
        {
            get
            {
                List<float> valoresParcelas = new List<float>();
                var valorParcelaAcumaldo = 0.0;
                var valorPorParcela = ValorPorParcela;
                for (int i = 1; i <= QuantidadeParcelas; i++)
                {
                    if (i == QuantidadeParcelas)
                        valoresParcelas.Add((float)Math.Round(Valor - valorParcelaAcumaldo, 2));
                    else
                        valoresParcelas.Add(valorPorParcela);

                    valorParcelaAcumaldo += valorPorParcela;
                }
                return valoresParcelas;
            }
        }

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
