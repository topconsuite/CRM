using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ContratoPagamentoDetalheCartaoScopes
    {
        public static bool EncontrarContratoPagamentoDetalheCartaoScopeIsValid(this ContratoPagamentoDetalheCartao contratoPagamentoDetalheCartao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(contratoPagamentoDetalheCartao, "contasAReceber-ContratoPagamentoDetalheCartao", "Detalhe do pagamento não encontrado!")
            );
        }

        public static bool EncontrarContratoPagamentoDetalheCartaoScopeIsValid(this ContratoPagamentoDetalheCartaoVersao contratoPagamentoDetalheCartao)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(contratoPagamentoDetalheCartao, "contasAReceber-ContratoPagamentoDetalheCartao", "Detalhe do pagamento não encontrado!")
            );
        }

        public static bool AprovaPagamentoAntecipadoCartaoIsValid(this ContratoPagamento contratoPagamento)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(contratoPagamento.Detalhes.Count, 1, "contasAReceber-ContratoPagamentoDetalheCartao", "Não é permitido mais que um detalhamento em uma única condição de pagamento para o tipo 'cartão'!")
            );
        }

        public static bool AprovaPagamentoAntecipadoCartaoIsValid(this ContratoPagamentoVersao contratoPagamento)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(contratoPagamento.Detalhes.Count, 1, "contasAReceber-ContratoPagamentoDetalheCartao", "Não é permitido mais que um detalhamento em uma única condição de pagamento para o tipo 'cartão'!")
            );
        }
    }
}
