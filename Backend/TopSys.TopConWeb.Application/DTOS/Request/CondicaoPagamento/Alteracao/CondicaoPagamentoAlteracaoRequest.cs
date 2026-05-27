using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao
{
    public class CondicaoPagamentoAlteracaoRequest
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public int QuantidadeParcelas { get; set; }

        public ICollection<CondicaoPagamentoParcelaDTO> Parcelas { get; set; }

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

        public string VencimentoFixoSemana { get; set; }

        public int DiaVencimentoFixoSemana { get; set; }

        public int DiaUltimoVencimento { get; set; }

        public string AnalisaFraude { get; set; }

        public string Ativo { get; set; }
        public string MesFixo30Dias { get; set; }
        public string RetencaoPrimeiraParcela { get; set; }

        public string TiposDeCobrancaCodigos { get; set; }
        public float MediaDias { get; set; }
    }
}
