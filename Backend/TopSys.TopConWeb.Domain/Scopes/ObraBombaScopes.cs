using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ObraBombaScopes
    {
        public const string TAG_VALOR_PROPOSTO_SEM_ALTERACAO = "sem_alteracao";

        public static bool DescontoMaximoScopeIsValid(this ObraBomba obraBomba, IBombaPreco bombaPreco)
        {
            var jaAprovado = obraBomba.AprovacaoObservacao != "";
            if (jaAprovado)
            {
                if (obraBomba.AprovacaoObservacao == TAG_VALOR_PROPOSTO_SEM_ALTERACAO)
                {
                    obraBomba.AprovacaoObservacao = "";
                }else
                    return true;
            }

            if (!obraBomba.BombaPropria && obraBomba.FaturamentoDireto) return true;

            var descontoValido = !((obraBomba.M3PrecoProposto < bombaPreco.M3PrecoValorMinimo
                || obraBomba.M3PropostoAte < bombaPreco.M3AteValorMinimo
                || obraBomba.TaxaMinimaPrecoProposto < bombaPreco.TaxaMinimaPrecoValorMinimo)
                && (obraBomba.BombaPropria || !obraBomba.FaturamentoDireto)
                && obraBomba.TipoCalculo != EBombaM3CalculoTipo.SemCobranca);

            if (descontoValido && obraBomba.BombaPropria)
            {
                descontoValido &= !((obraBomba.HoraPrecoProposto < bombaPreco.HoraPrecoValorMinimo
                || obraBomba.HoraPropostoAte < bombaPreco.HoraAteValorMinimo
                || obraBomba.HoraTaxaMinimaPrecoProposto < bombaPreco.HoraTaxaMinimaPrecoValorMinimo)
                && obraBomba.HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca);
            }

            if (descontoValido || obraBomba.Justificativa != "") return descontoValido;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(descontoValido, $"obraBomba-{obraBomba.Sequencia}-Desconto", $"Desconto acima do permitido!")
            );
        }

        public static bool DescontoMaximoScopeIsValid(this ObraBombaVersao obraBomba, IBombaPreco bombaPreco)
        {
            var jaAprovado = obraBomba.AprovacaoObservacao != "";
            if (jaAprovado)
            {
                if (obraBomba.AprovacaoObservacao == TAG_VALOR_PROPOSTO_SEM_ALTERACAO)
                {
                    obraBomba.AprovacaoObservacao = "";
                }
                else
                    return true;
            }

            if (!obraBomba.BombaPropria && obraBomba.FaturamentoDireto) return true;

            var descontoValido = (!(obraBomba.M3PrecoProposto < bombaPreco.M3PrecoValorMinimo
                || obraBomba.M3PropostoAte < bombaPreco.M3AteValorMinimo
                || obraBomba.TaxaMinimaPrecoProposto < bombaPreco.TaxaMinimaPrecoValorMinimo)
                && (obraBomba.BombaPropria || !obraBomba.FaturamentoDireto)
                && obraBomba.TipoCalculo != EBombaM3CalculoTipo.SemCobranca);

            if (descontoValido && obraBomba.BombaPropria)
            {
                descontoValido &= !((obraBomba.HoraPrecoProposto < bombaPreco.HoraPrecoValorMinimo
                || obraBomba.HoraPropostoAte < bombaPreco.HoraAteValorMinimo
                || obraBomba.HoraTaxaMinimaPrecoProposto < bombaPreco.HoraTaxaMinimaPrecoValorMinimo)
                && obraBomba.HoraTipoCalculo != EBombaHoraCalculoTipo.SemCobranca);
            }

            if (descontoValido || obraBomba.Justificativa != "") return descontoValido;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(descontoValido, $"obraBomba-{obraBomba.Sequencia}-Desconto", $"Desconto acima do permitido!")
            );
        }

        public static bool BombaPrecoTabelaIsValid(this ObraBomba obraBomba, IBombaPreco bombaPreco)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(bombaPreco, $"obraBomba-{obraBomba.Sequencia}-BombaPrecoTabela",
                    "Não existe tabela de preço para a bomba informada!")
            );
        }

        public static bool BombaPrecoTabelaIsValid(this ObraBombaVersao obraBomba, IBombaPreco bombaPreco)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(bombaPreco, $"obraBomba-{obraBomba.Sequencia}-BombaPrecoTabela",
                    "Não existe tabela de preço para a bomba informada!")
            );
        }

        public static bool BombaProgramacaoScopeIsValid(this ObraBomba obraBomba, int? oldBombaTipo)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(obraBomba.BombaTipoCodigo ?? 0, oldBombaTipo ?? 0,
                    $"obraBomba-{obraBomba.Sequencia}-BombaTipo", "Não é possível alterar bomba para o qual já existe programação!")
            );
        }

        public static bool BombaProgramacaoScopeIsValid(this ObraBombaVersao obraBomba, int? oldBombaTipo)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(obraBomba.BombaTipoCodigo ?? 0, oldBombaTipo ?? 0,
                    $"obraBomba-{obraBomba.Sequencia}-BombaTipo", "Não é possível alterar bomba para o qual já existe programação!")
            );
        }
    }
}
