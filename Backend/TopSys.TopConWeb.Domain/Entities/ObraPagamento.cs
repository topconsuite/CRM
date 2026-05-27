using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraPagamento
    {
        public int UsinaCodigo { get; set; }
        public Usina Usina { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public int CondicaoPagamentoCodigo { get; set; }
        public CondicaoPagamento CondicaoPagamento { get; set; }

        public int TipoCobrancaCodigo { get; set; }
        public TipoCobranca TipoCobranca { get; set; }

        public string Forma { get; set; }

        public string ValorFixoSimNao { get; set; }
        public virtual bool ValorFixo
        {
            get { return (ValorFixoSimNao ?? "").Equals("S"); }
            set { ValorFixoSimNao = value ? "S" : "N"; }
        }

        public float Valor { get; set; }

        public float Percentual { get; set; }

        public string NecessitaAprovacaoSimNao { get; set; }
        public virtual bool NecessitaAprovacao
        {
            get { return !(NecessitaAprovacaoSimNao ?? "").Equals("N"); }
            set { NecessitaAprovacaoSimNao = (value ? "S" : "N"); }
        }

        public string AtivoSimNao { get; set; }
        public virtual bool Ativo
        {
            get { return (AtivoSimNao ?? "").Equals("S"); }
            set { AtivoSimNao = value ? "S" : "N"; }
        }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public string IdAprovacao { get; set; }

        public virtual ICollection<ObraPagamentoDetalhe> Detalhes { get; set; }

        public EStatusAprovacao StatusAprovacao { get; set; }

        public string LogObservacao { get; set; }

        public abstract void AtualizaStatusAprovacao();

        public bool FormaPagamentoECartaoCredito()
        {
            return Forma == "CC";
        }
    }
}
