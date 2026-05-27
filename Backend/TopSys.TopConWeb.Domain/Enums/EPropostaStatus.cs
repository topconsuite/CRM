using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Enums
{
    public enum EPropostaStatus
    {
        Nenhum = 0,
        Andamento = 9101,
        AguardandoAprovacaoComercial = 9102,
        AguardandoAprovacaoEngenharia = 9103,
        Enviada = 9104,
        Perdida = 9105,
        AprovadaPeloCliente = 9106,
        ContratoGerado = 9107,
        ReprovadaComercialmente = 9108,
        Reprovada = 9109
    }
}
