using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class ContratoPagamentoDetalheBoletoScopes
    {
        public static bool DesaprovacaoContratoPagamentoDetalheBoletoScopeIsValid(this ContratoPagamentoDetalheBoleto contratoPagamentoDetalheBoleto)
        {
            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(contratoPagamentoDetalheBoleto.DataLiquidacao == null, "ContratoPagamentoDetalheBoleto", $"Não é Possível Desaprovar Condição De Pagamento! Boleto Liquidado Dia: {contratoPagamentoDetalheBoleto.DataLiquidacao?.ToString("dd/MM/yyyy")}")
           );
        }
    }
}
