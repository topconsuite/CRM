using System;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ObraTracoScopes
    {
        public static bool DescontoMaximoScopeIsValid(this ObraTraco obraTraco, float valorSugerido, float percentualDescontoMaximo)
        {
            var jaAprovado = obraTraco.AprovacaoObservacao != "";
            if (jaAprovado) return true;

            var valorMinimo = (float)Math.Round(valorSugerido * (1f - (percentualDescontoMaximo / 100f)), 2);

            var descontoValido = (obraTraco.M3PrecoProposto >= valorMinimo);

            if (descontoValido || obraTraco.Justificativa != "") return descontoValido;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(descontoValido, $"obraTraco-{obraTraco.Sequencia}-Desconto",
                    $"Desconto acima do permitido! Valor mínimo: R${valorMinimo}")
            );
        }

        public static bool DescontoMaximoScopeIsValid(this ObraTracoVersao obraTraco, float valorSugerido, float percentualDescontoMaximo)
        {
            var jaAprovado = obraTraco.AprovacaoObservacao != "";
            if (jaAprovado) return true;

            var valorMinimo = (float)Math.Round(Math.Round(valorSugerido, 2) * (1f - (percentualDescontoMaximo / 100f)), 2);

            var descontoValido = (obraTraco.M3PrecoProposto >= valorMinimo);

            if (descontoValido || obraTraco.Justificativa != "") return descontoValido;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(descontoValido, $"obraTraco-{obraTraco.Sequencia}-Desconto",
                    $"Desconto acima do permitido! Valor mínimo: R${valorMinimo}")
            );
        }

        public static bool DescontoMaximoPrecoReajustadoScopeIsValid(this ObraTraco obraTraco, float valorSugerido, float percentualDescontoMaximo)
        {
            var jaAprovado = obraTraco.AprovacaoObservacao != "";
            if (jaAprovado) return true;

            var valorMinimo = (float)Math.Round(Math.Round(valorSugerido, 2) * (1f - (percentualDescontoMaximo / 100f)), 2);

            var descontoValido = (obraTraco.PrecoReajustadoAtual >= valorMinimo);

            if (descontoValido || obraTraco.Justificativa != "") return descontoValido;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(descontoValido, $"obraTraco-{obraTraco.Sequencia}-Desconto",
                    $"Desconto no reajuste acima do permitido! Valor mínimo: R${valorMinimo}")
            );
        }

        public static bool DescontoMaximoPrecoReajustadoScopeIsValid(this ObraTracoVersao obraTraco, float valorSugerido, float percentualDescontoMaximo)
        {
            var jaAprovado = obraTraco.AprovacaoObservacao != "";
            if (jaAprovado) return true;

            var valorMinimo = (float)Math.Round(valorSugerido * (1f - (percentualDescontoMaximo / 100f)), 2);

            var descontoValido = (obraTraco.PrecoReajustadoAtual >= valorMinimo);

            if (descontoValido || obraTraco.Justificativa != "") return descontoValido;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(descontoValido, $"obraTraco-{obraTraco.Sequencia}-Desconto",
                    $"Desconto no reajuste acima do permitido! Valor mínimo: R${valorMinimo}")
            );
        }

        public static bool TracoPrecoTabelaIsValid(this ObraTraco obraTraco, TracoPreco tracoPreco)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(tracoPreco, $"obraTraco-{obraTraco.Sequencia}-TracoPrecoTabela",
                    "Não existe tabela de preço para o traço informado!")
            );
        }

        public static bool TracoPrecoTabelaIsValid(this ObraTracoVersao obraTraco, TracoPreco tracoPreco)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(tracoPreco, $"obraTraco-{obraTraco.Sequencia}-TracoPrecoTabela",
                    "Não existe tabela de preço para o traço informado!")
            );
        }

        public static bool TracoProgramacaoScopeIsValid(this ObraTraco obraTraco, Programacao programacao)
        {
            var alertaAlteracaoTraco = "Não é possível alterar traço para o qual já existe programação!";

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(obraTraco.ResistenciaTipoCodigo, programacao.ResistenciaTipoCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoResistenciaTipo", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.Fck, programacao.Mpa,
                    $"obraTraco-{obraTraco.Sequencia}-TracoMpa", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.Consumo, programacao.Consumo,
                    $"obraTraco-{obraTraco.Sequencia}-TracoConsumo", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.PedraCodigo, programacao.PedraCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoPedra", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.SlumpCodigo, programacao.SlumpCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoSlump", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.UsoCodigo, programacao.UsoCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoUso", alertaAlteracaoTraco)
            );
        }

        public static bool TracoProgramacaoScopeIsValid(this ObraTracoVersao obraTraco, Programacao programacao)
        {
            var alertaAlteracaoTraco = "Não é possível alterar traço para o qual já existe programação!";

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(obraTraco.ResistenciaTipoCodigo, programacao.ResistenciaTipoCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoResistenciaTipo", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.Fck, programacao.Mpa,
                    $"obraTraco-{obraTraco.Sequencia}-TracoMpa", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.Consumo, programacao.Consumo,
                    $"obraTraco-{obraTraco.Sequencia}-TracoConsumo", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.PedraCodigo, programacao.PedraCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoPedra", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.SlumpCodigo, programacao.SlumpCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoSlump", alertaAlteracaoTraco),
                AssertionConcern.AssertAreEquals(obraTraco.UsoCodigo, programacao.UsoCodigo ?? 0,
                    $"obraTraco-{obraTraco.Sequencia}-TracoUso", alertaAlteracaoTraco)
            );
        }

        public static bool TracoReprovado(this ObraTraco obraTraco)
        {
            if (obraTraco.AprovacaoVerbal == "S" && obraTraco.AprovacaoOperacao == "X" && obraTraco.AprovacaoCiente == "N" && obraTraco.AprovacaoObservacao != "")
                return true;

            return false;
        }

        public static bool TracoReprovado(this ObraTracoVersao obraTraco)
        {
            if (obraTraco.AprovacaoVerbal == "S" && obraTraco.AprovacaoOperacao == "X" && obraTraco.AprovacaoCiente == "N" && obraTraco.AprovacaoObservacao != "")
                return true;

            return false;
        }
    }
}
