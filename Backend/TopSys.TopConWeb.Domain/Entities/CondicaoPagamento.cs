using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class CondicaoPagamento
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeParcelas { get; set; }

        public ICollection<CondicaoPagamentoParcela> Parcelas { get; set; }

        public virtual float PrazoMedio {
            get
            {
                return (Parcelas != null && Parcelas.Count > 0) ? (float)Parcelas.Average(t => t.Dias) : 0f;
            }
        }

        public int Vencimento1Parcela { get; set; }
        public int Vencimento2Parcela { get; set; }
        public int Vencimento3Parcela { get; set; }
        public int Vencimento4Parcela { get; set; }
        public int Vencimento5Parcela { get; set; }
        public int Vencimento6Parcela { get; set; }
        public int Vencimento7Parcela { get; set; }
        public int Vencimento8Parcela { get; set; }
        public int Vencimento9Parcela { get; set; }
        public int Vencimento10Parcela { get; set; }
        public int Vencimento11Parcela { get; set; }
        public int Vencimento12Parcela { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public string CondicaoDaCobrancaCod { get; set; }

        public CadastroDiverso CondicaoDaCobranca { get; set; }

        public string VencimentoFixoSemana { get; set; }

        public int? DiaVencimentoFixoSemana { get; set; } = 0;

        public int? DiaUltimoVencimento { get; set; } = 0;

        public string AnalisaFraude { get; set; }

        public string Ativo { get; set; }
        public string MesFixo30Dias { get; set; }
        public string RetencaoPrimeiraParcela { get; set; }

        public string TiposDeCobrancaCodigos { get; set; }

        public string IdExterno { get; set; }

        public string DescricaoCompleta { get; set; }

        public float MediaDias { get; set; }
    }
}
