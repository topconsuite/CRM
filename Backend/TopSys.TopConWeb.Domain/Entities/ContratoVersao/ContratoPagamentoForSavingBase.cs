using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ContratoPagamentoForSavingBase<TContratoPagamentoDetalhe> : ObraPagamento
    {
        public ContratoPagamentoForSavingBase() : base() { }

        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }

        public float ValorApropriado { get; set; }

        public new virtual ICollection<TContratoPagamentoDetalhe> Detalhes { get; set; }

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
