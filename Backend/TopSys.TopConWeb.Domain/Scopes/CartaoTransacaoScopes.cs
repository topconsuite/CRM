using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class CartaoTransacaoScopes
    {
        public static bool CartaoTransacaoNaoProcessadoScopeIsValid(this CartaoTransacao cartaoTransacao)
        {
            var processado = (int)EContasAReceberStatusProcesso.Processado;

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(cartaoTransacao, "cartaoTransacao", "Transacao não encontrada!"),
                AssertionConcern.AssertAreNotEquals(processado, cartaoTransacao?.StatusProcesso ?? 0, "Status Processo","Contas a receber já gerado para esta transação")
            );
        }
    }
}
