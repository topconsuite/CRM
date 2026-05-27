using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICalculoImpostosService
    {
        float CalcularIss(Obra obra, float valorM3, float valorMaterial, bool calculoBomba = false, bool calcularISSParaPrecoSugerido = false);
        float CalcularIss(ObraVersao obra, float valorM3, float valorMaterial, bool calculoBomba = false, bool calcularISSParaPrecoSugerido = false);
        float CalcularIssServico(float valorM3, float aliquota, float valorMaterial);
        float CalcularImpostoSomenteSobreAliquota(float valorM3, float aliquota);
        float CalcularIssPorcentagemFixaSobreValorFatura(float valorM3, float aliquota, float deducao);
        float CalcularIssDeducaoSobreMaterial(float valorM3, float aliquota, float deducao, float valorMaterial);
        float CalcularIssDeducaoSobreMaterialLimitado(float valorM3, float aliquota, float deducao, float valorMaterial);
    }
}
