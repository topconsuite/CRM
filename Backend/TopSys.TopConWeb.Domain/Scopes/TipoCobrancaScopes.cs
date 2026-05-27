using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class TipoCobrancaScopes
    {
        public static bool EncontrarTipoCobrancaScopeIsValid(this TipoCobranca tipoCobranca)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(tipoCobranca, "TipoCobranca", "Tipo de cobranca não encontrado!")
            );
        }
    }
}
