using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class EnderecoScopes
    {
        public static bool EnderecoScopeIsValid(this Endereco endereco, string campo)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(endereco, campo, "CEP inexistente!")
            );
        }
    }
}
