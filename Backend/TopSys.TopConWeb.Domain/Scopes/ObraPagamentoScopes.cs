using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ObraPagamentoScopes
    {
        const int MAX_SEQ = 99;

        public static bool PagamentoSequenciaIsValid(this ObraPagamento pagamento)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(MAX_SEQ, pagamento.Sequencia, $"pagamento-{pagamento.Sequencia}-Sequencia",
                    $"Sequencia máxima de pagamentos é {MAX_SEQ}!"),
                AssertionConcern.AssertIsGreaterThan(pagamento.Sequencia, 0, $"pagamento-{pagamento.Sequencia}-Sequencia",
                    $"Sequencia do pagamento deve ser maior que 0!")
            );
        }

        public static bool PagamentoDetalheSequenciaIsValid(this ObraPagamentoDetalhe detalhe)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(MAX_SEQ, detalhe.DetalheSequencia, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DetalheSequencia",
                    $"Sequencia máxima de pagamento/detalhamentos é {MAX_SEQ}!"),
                AssertionConcern.AssertIsGreaterThan(detalhe.DetalheSequencia, 0, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DetalheSequencia",
                    $"Sequencia do pagamento/detalhamento deve ser maior que 0!")
            );
        }

        public static bool PagamentoDetalheDataContaConciliadaIsValid(this ContratoPagamentoDetalhe detalhe, ContratoPagamento pagamento, IEnumerable<ContratoPagamento> pagamentos, Conta conta, bool vincularComLancamentoExistente = false)
        {
            var contaJaConciliadaNaData = conta?.IsConciliadaNaData(detalhe.DataTitulo()) ?? false;

            var aprovacaoUnica = pagamento.IsAprovacaoUnica(pagamentos);

            var isValid = (!contaJaConciliadaNaData && !vincularComLancamentoExistente)
                || (contaJaConciliadaNaData && vincularComLancamentoExistente);

            return AssertionConcern.IsSatisfiedBy
            (
                // utilizei "isValid || !aprovacaoUnica" porque quando cair na validação de "aprovação única" não pode apresentar a outra mensagem
                AssertionConcern.AssertTrue(isValid || !aprovacaoUnica, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DataContaConciliada",
                    $"Não é permitido gerar movimento de banco para a data {detalhe.DataTitulo()?.ToString("dd/MM/yyyy")}" +
                    $" pois o banco já está conciliado nesta data!\nDeseja vincular com lançamento existente?"),
                AssertionConcern.AssertTrue(aprovacaoUnica || !contaJaConciliadaNaData, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DataContaConciliadaAprovacaoUnica",
                    $"Um ou mais pagamentos necessitam de vinculação com lançamentos bancários existentes! Estes pagamentos devem ser aprovados individualmente!")
            );
        }

        public static bool PagamentoDetalheDataContaConciliadaIsValid(this ContratoPagamentoDetalheVersao detalhe, ContratoPagamentoVersao pagamento, IEnumerable<ContratoPagamentoVersao> pagamentos, Conta conta, bool vincularComLancamentoExistente = false)
        {
            var contaJaConciliadaNaData = conta?.IsConciliadaNaData(detalhe.DataTitulo()) ?? false;

            var aprovacaoUnica = pagamento.IsAprovacaoUnica(pagamentos);

            var isValid = (!contaJaConciliadaNaData && !vincularComLancamentoExistente)
                || (contaJaConciliadaNaData && vincularComLancamentoExistente);

            return AssertionConcern.IsSatisfiedBy
            (
                // utilizei "isValid || !aprovacaoUnica" porque quando cair na validação de "aprovação única" não pode apresentar a outra mensagem
                AssertionConcern.AssertTrue(isValid || !aprovacaoUnica, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DataContaConciliada",
                    $"Não é permitido gerar movimento de banco para a data {detalhe.DataTitulo()?.ToString("dd/MM/yyyy")}" +
                    $" pois o banco já está conciliado nesta data!\nDeseja vincular com lançamento existente?"),
                AssertionConcern.AssertTrue(aprovacaoUnica || !contaJaConciliadaNaData, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DataContaConciliadaAprovacaoUnica",
                    $"Um ou mais pagamentos necessitam de vinculação com lançamentos bancários existentes! Estes pagamentos devem ser aprovados individualmente!")
            );
        }

        public static bool PagamentoDetalheVinculoMovimentoBancoIsValid(this ContratoPagamentoDetalhe detalhe, IEnumerable<MovimentoBanco> movimentosBancoAVincular, IEnumerable<MovimentoBanco> movimentosBancoAtual)
        {
            var saldoIsValid = movimentosBancoAVincular.Count() == 0 || movimentosBancoAVincular.Sum(t => t.Saldo) >= detalhe.Valor;
            var houveAlteracaoNoMovimento = false;

            foreach (var movimentoBancoAvincular in movimentosBancoAVincular)
            {
                var movimentoBancoAtual = movimentosBancoAtual
                    .FirstOrDefault(t => t.Id == movimentoBancoAvincular.Id && Math.Round(t.Saldo, 2) == Math.Round(movimentoBancoAvincular.Saldo, 2));

                houveAlteracaoNoMovimento |= (movimentoBancoAtual == null);
            }

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(saldoIsValid, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-VinculoMovimentoBancoSaldo",
                    $"Não é possível vincular pois o saldo total do(s) lançamento(s) selecionado(s) é inferior ao valor do pagamento."),
                AssertionConcern.AssertTrue(!houveAlteracaoNoMovimento, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-VinculoMovimentoBancoAlterado",
                    $"Não é possível vincular pois um ou mais lançamentos selecionados foram alterados.")
            );
        }

        public static bool PagamentoDetalheVinculoMovimentoBancoIsValid(this ContratoPagamentoDetalheVersao detalhe, IEnumerable<MovimentoBanco> movimentosBancoAVincular, IEnumerable<MovimentoBanco> movimentosBancoAtual)
        {
            var saldoIsValid = movimentosBancoAVincular.Count() == 0 || movimentosBancoAVincular.Sum(t => t.Saldo) >= detalhe.Valor;
            var houveAlteracaoNoMovimento = false;

            foreach (var movimentoBancoAvincular in movimentosBancoAVincular)
            {
                var movimentoBancoAtual = movimentosBancoAtual
                    .FirstOrDefault(t => t.Id == movimentoBancoAvincular.Id && Math.Round(t.Saldo, 2) == Math.Round(movimentoBancoAvincular.Saldo, 2));

                houveAlteracaoNoMovimento |= (movimentoBancoAtual == null);
            }

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(saldoIsValid, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-VinculoMovimentoBancoSaldo",
                    $"Não é possível vincular pois o saldo total do(s) lançamento(s) selecionado(s) é inferior ao valor do pagamento."),
                AssertionConcern.AssertTrue(!houveAlteracaoNoMovimento, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-VinculoMovimentoBancoAlterado",
                    $"Não é possível vincular pois um ou mais lançamentos selecionados foram alterados.")
            );
        }

        public static bool PagamentoDetalheDataFuturaIsValid(this ContratoPagamentoDetalhe detalhe)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(DateTime.Today, detalhe.DataTitulo(), $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DataFutura",
                    $"Data do pagamento maior que a data atual!")
            );
        }

        public static bool PagamentoDetalheDataFuturaIsValid(this ContratoPagamentoDetalheVersao detalhe)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(DateTime.Today, detalhe.DataTitulo(), $"pagamentoDetalhe-{detalhe.DetalheSequencia}-DataFutura",
                    $"Data do pagamento maior que a data atual!")
            );
        }

        public static bool PagamentoModificadoIsValid(this ContratoPagamento pagamentoAtual, ContratoPagamento pagamentoAnterior)
        {
            var infoAtual = string.Join(";", pagamentoAtual?.Detalhes?.OrderBy(t => t.DetalheSequencia)?.Select(t => t.InfoString()) ?? new string[] { });
            var infoAnterior = string.Join(";", pagamentoAnterior?.Detalhes?.OrderBy(t => t.DetalheSequencia)?.Select(t => t.InfoString()) ?? new string[] { });
            var qtdDetalhamentosAtual = pagamentoAtual?.Detalhes?.Count() ?? 0;
            var qtdDetalhamentosAnterior = pagamentoAnterior?.Detalhes?.Count() ?? 0;

            var aprovadoAnteriormente = (pagamentoAnterior.StatusAprovacao != Enums.EStatusAprovacao.NaoNecessita && pagamentoAnterior.IdAprovacao != "");

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(!aprovadoAnteriormente, $"pagamento-{pagamentoAtual.Sequencia}-Aprovado", $"Pagamento já aprovado!"),
                AssertionConcern.AssertAreEquals(infoAtual, infoAnterior, $"pagamento-{pagamentoAtual.Sequencia}-Alterado", $"Pagamento foi alterado!"),
                AssertionConcern.AssertAreEquals(qtdDetalhamentosAtual, qtdDetalhamentosAnterior, $"pagamento-{pagamentoAtual.Sequencia}-Alterado", $"Houve alteração na quantidade de detalhamentos informados!")
            );
        }

        public static bool PagamentoModificadoIsValid(this ContratoPagamentoVersao pagamentoAtual, ContratoPagamentoVersao pagamentoAnterior)
        {
            var infoAtual = string.Join(";", pagamentoAtual?.Detalhes?.OrderBy(t => t.DetalheSequencia)?.Select(t => t.InfoString()) ?? new string[] { });
            var infoAnterior = string.Join(";", pagamentoAnterior?.Detalhes?.OrderBy(t => t.DetalheSequencia)?.Select(t => t.InfoString()) ?? new string[] { });
            var qtdDetalhamentosAtual = pagamentoAtual?.Detalhes?.Count() ?? 0;
            var qtdDetalhamentosAnterior = pagamentoAnterior?.Detalhes?.Count() ?? 0;

            var aprovadoAnteriormente = (pagamentoAnterior.StatusAprovacao != Enums.EStatusAprovacao.NaoNecessita && pagamentoAnterior.IdAprovacao != "");

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(!aprovadoAnteriormente, $"pagamento-{pagamentoAtual.Sequencia}-Aprovado", $"Pagamento já aprovado!"),
                AssertionConcern.AssertAreEquals(infoAtual, infoAnterior, $"pagamento-{pagamentoAtual.Sequencia}-Alterado", $"Pagamento foi alterado!"),
                AssertionConcern.AssertAreEquals(qtdDetalhamentosAtual, qtdDetalhamentosAnterior, $"pagamento-{pagamentoAtual.Sequencia}-Alterado", $"Houve alteração na quantidade de detalhamentos informados!")
            );
        }

        public static bool PagamentosModificadosIsValid(this Obra obraAtual, Obra obraAnterior)
        {
            var qtdPagamentosAtual = obraAtual?.ContratoPagamentos?.Count() ?? 0;
            var qtdPagamentosAnterior = obraAnterior?.ContratoPagamentos?.Count() ?? 0;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(qtdPagamentosAtual, qtdPagamentosAnterior, $"pagamentos-Alterados", $"Houve alteração nos pagamentos informados!")
            );
        }

        public static bool PagamentosModificadosIsValid(this ObraVersao obraAtual, ObraVersao obraAnterior)
        {
            var qtdPagamentosAtual = obraAtual?.ContratoPagamentos?.Count() ?? 0;
            var qtdPagamentosAnterior = obraAnterior?.ContratoPagamentos?.Count() ?? 0;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(qtdPagamentosAtual, qtdPagamentosAnterior, $"pagamentos-Alterados", $"Houve alteração nos pagamentos informados!")
            );
        }

        public static bool PagamentosAtivosIsValid(this IEnumerable<PropostaPagamento> obraPagamentos)
        {

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(obraPagamentos?.Where(t => t.Ativo).Count() ?? 0, 1, $"pagamentos-ativos", $"A Proposta deve possuir pelo menos 1 pagamento ativo!")
            );
        }

        public static bool PagamentosAtivosIsValid(this IEnumerable<PropostaPagamentoVersao> obraPagamentos)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(obraPagamentos?.Where(t => t.Ativo).Count() ?? 0, 1, $"pagamentos-ativos", $"A Proposta deve possuir pelo menos 1 pagamento ativo!")
            );
        }

        public static bool PagamentoDetalheMesFechadoIsValid(this ContratoPagamentoDetalhe detalhe, DateTime? mesAberto)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(detalhe.DataTitulo(), mesAberto, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-MesFechado", $"Data do pagamento pertence a mês fechado no financeiro!")
            );
        }

        public static bool PagamentoDetalheMesFechadoIsValid(this ContratoPagamentoDetalheVersao detalhe, DateTime? mesAberto)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterOrEqualThan(detalhe.DataTitulo(), mesAberto, $"pagamentoDetalhe-{detalhe.DetalheSequencia}-MesFechado", $"Data do pagamento pertence a mês fechado no financeiro!")
            );
        }

        public static bool EmpresaPortadorIsValid(this Obra obra, Portador portador, ContratoPagamento pagamentoAtual)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(portador.ContaEmpresaCodigo?? 0, obra.UsinaEntrega.EmpresaCodigo, "Portador", $"Portador do pagamento {pagamentoAtual.TipoCobranca.Descricao} ({pagamentoAtual.Valor}) não pertence a empresa da usina principal do Contrato")
            );
        }

        public static bool EmpresaPortadorIsValid(this ObraVersao obra, Portador portador, ContratoPagamentoVersao pagamentoAtual)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(portador.ContaEmpresaCodigo ?? 0, obra.UsinaEntrega.EmpresaCodigo, "Portador", $"Portador do pagamento {pagamentoAtual.TipoCobranca.Descricao} ({pagamentoAtual.Valor}) não pertence a empresa da usina principal do Contrato")
            );
        }

        public static bool ContaPortadorIsValid(this Conta conta, ContratoPagamento pagamentoAtual)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(conta, "Banco do Portador", $"Portador do pagamento {pagamentoAtual.TipoCobranca.Descricao} ({pagamentoAtual.Valor}) não possui banco configurado!")
            );
        }

        public static bool ContaPortadorIsValid(this Conta conta, ContratoPagamentoVersao pagamentoAtual)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(conta, "Banco do Portador", $"Portador do pagamento {pagamentoAtual.TipoCobranca.Descricao} ({pagamentoAtual.Valor}) não possui banco configurado!")
            );
        }
    }
}
