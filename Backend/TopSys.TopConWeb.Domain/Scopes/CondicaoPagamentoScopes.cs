using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Helpers;
using Topsys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Constants.CondicaoPagamento;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
	public static class CondicaoPagamentoScopes
    {
		public static bool CondicaoPagamentoToAddIsValid(this CondicaoPagamento condicaoPagamento, IEnumerable<CondicaoPagamento> condicoesPagamentos )
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(condicoesPagamentos.Where(t => t.Descricao == condicaoPagamento.Descricao).Count(),
                    0, "condicaoPagamento", "Condição de pagamento Já existente!"),
                AssertionConcern.AssertTrue(Math.Round(condicaoPagamento.Parcelas.Sum(t => t.Percentual)) == 100,
                   "condicaoPagamento", "A Somatória dos Percentuais das Parcelas Deve ser Igual a 100%!")
            );
        }

        public static bool CondicaoPagamentoToUpdateIsValid(this CondicaoPagamento condicaoPagamento)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(Math.Round(condicaoPagamento.Parcelas.Sum(t => t.Percentual)) == 100,
                   "condicaoPagamento", "A Somatória dos Percentuais das Parcelas Deve ser Igual a 100%!")
            );
        }
        
	}
}
