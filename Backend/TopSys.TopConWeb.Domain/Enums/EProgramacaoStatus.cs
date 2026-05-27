using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Enums
{
    public enum EProgramacaoStatus
    {
        AguardandoConfirmacao = 9161,
        Programado = 9162,
        Rejeitado = 9163,
        Cancelada = 9164,
        Revalidacao = 9165,
        AguardandoAnaliseLimiteCredito = 9166,
        LimiteCreditoInsuficiente = 9167,
        AprovacaoInadimplente = 9168
    }
}
