using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ContratoPagamentoBase<TContratoPagamentoDetalhe> : ObraPagamento
        where TContratoPagamentoDetalhe : ContratoPagamentoDetalheBase
    {
        public ContratoPagamentoBase() : base() { }
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

        public long ChaveContrato
        {
            get
            {
                return long.Parse($"{ContratoAno.ToString().PadLeft(2, '0')}{ContratoNumero.ToString().PadLeft(5, '0')}");
            }
        }

        public bool IsAprovacaoUnica(IEnumerable<ContratoPagamento> contratoPagamentos)
        {
            var detalhe = Detalhes?.FirstOrDefault();
            var quantidadePagamentosAprovados = contratoPagamentos.Where(t => t.StatusAprovacao == Enums.EStatusAprovacao.Aprovado).Count();

            return (Detalhes?.Count == 1 && StatusAprovacao == EStatusAprovacao.Aprovado && quantidadePagamentosAprovados == 1);
        }

        public bool IsAprovacaoUnica(IEnumerable<ContratoPagamentoVersao> contratoPagamentos)
        {
            var detalhe = Detalhes?.FirstOrDefault();
            var quantidadePagamentosAprovados = contratoPagamentos.Where(t => t.StatusAprovacao == Enums.EStatusAprovacao.Aprovado).Count();

            return (Detalhes?.Count == 1 && StatusAprovacao == EStatusAprovacao.Aprovado && quantidadePagamentosAprovados == 1);
        }
    }
}
