using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ContratoScopes
    {

        public static bool ContratoIsEncerrado(this Contrato contrato)
        {

            return !AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(contrato.DataEncerramento == null, "contrato.DataEncerramento", "Contrato Já Encerrado!"),
                AssertionConcern.AssertTrue(contrato.Fechado, "contrato.Fechado", "Contrato Não Fechado!")
            );

        }

    }
}
