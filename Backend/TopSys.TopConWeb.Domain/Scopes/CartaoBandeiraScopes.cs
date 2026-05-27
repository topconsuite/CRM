using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class CartaoBandeiraScopes
    {
        public static bool EncontrarCartaoBandeiraScopeIsValid(this CartaoBandeira cartaoBandeira)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(cartaoBandeira, "cartaoBandeira", "Bandeira do cartão não encontrado!")
            );
        }
    }
}
