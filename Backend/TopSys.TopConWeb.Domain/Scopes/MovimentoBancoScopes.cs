using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class MovimentoBancoScopes
    {
        public static bool OperacaoScopeIsValid(this MovimentoBanco movimentoBanco)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertIsGreaterThan(movimentoBanco.OperacaoCodigo, 0, "movimentoBanco-Operacao", "Operação de movimento de banco de adiantamento de cliente não configurada para a empresa selecionada!")
            );
        }
    }
}
