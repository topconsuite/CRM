using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Enums
{
    public enum EBaseCalculoIss
    {
        Servico = 1,
        TotalFatura = 2,
        PorcentagemFixaSobreValorFatura = 3,
        PorcentagemDeducaoSobreMaterial = 4,
        PorcentagemDeducaoMaterialLimitado = 5
    }
}
