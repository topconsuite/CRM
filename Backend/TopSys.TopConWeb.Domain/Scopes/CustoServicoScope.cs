using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class CustoServicoScope
    {
        public static bool CustoServicoScopeIsValid(this CustoServico custoServico)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(custoServico, "CustoServico", "Custo de serviço não configurado para a usina de entrega!Favor realizar a configuração no modulo Engenharia, aplicação 6024")
            );
        }
    }
}
