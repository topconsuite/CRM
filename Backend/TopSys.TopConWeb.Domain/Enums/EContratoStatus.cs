using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Enums
{
    public enum EContratoStatus
    {
        Inexistente = 0,
        NaoGerado = 9131,
        PreAnalise = 9132,
        Reprovado = 9133,
        EmAnalise = 9134,
        Pendente = 9135,
        Aprovado = 9136,
        AguardandoConfirmacaoPagamento = 9137,
        Cancelado = 9138,
        AguardandoDataProgramacao = 9139,
        RevalidacaoCadastro = 9140,
        AguardandoDadosPagamento = 9141,
        AguardandoAprovacaoComercial = 9144,
        Encerrado = 9145
    }
}
