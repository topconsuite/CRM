using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class PropostaPagamentoBase<TPropostaPagamentoDetalhe> : ObraPagamento
    {
        public PropostaPagamentoBase() : base() { }

        public int PropostaAno { get; set; }
        public int PropostaNumero { get; set; }

        public new virtual ICollection<TPropostaPagamentoDetalhe> Detalhes { get; set; }

        public override void AtualizaStatusAprovacao()
        {
            var pendente = NecessitaAprovacao && IdAprovacao == ""
                    && (Detalhes?.Count > 0 || TipoCobranca?.Forma == "CT");

            if (pendente)
                StatusAprovacao = EStatusAprovacao.Pendente;
            else
                StatusAprovacao = EStatusAprovacao.NaoNecessita;
        }
    }
}
