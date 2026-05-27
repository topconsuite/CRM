using System;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IBombaPreco
    {
        int BombaTipoCodigo { get; set; }
        CadastroGeral BombaTipo { get; set; }

        DateTime DataInicioVigencia { get; set; }

        int M3Ate { get; set; }

        float TaxaMinimaPreco { get; set; }

        float M3Preco { get; set; }

        int M3AteValorMinimo { get; set; }

        float TaxaMinimaPrecoPercentualDescontoMaximo { get; set; }
        float TaxaMinimaPrecoValorMinimo { get; set; }

        float M3PrecoPercentualDescontoMaximo { get; set; }
        float M3PrecoValorMinimo { get; set; }

        EBombaM3CalculoTipo TipoCalculo { get; set; }

        float HoraAte { get; set; }

        float HoraTaxaMinimaPreco { get; set; }

        float HoraPreco { get; set; }

        float HoraAteValorMinimo { get; set; }

        float HoraTaxaMinimaPrecoValorMinimo { get; set; }

        float HoraPrecoValorMinimo { get; set; }

        EBombaHoraCalculoTipo HoraTipoCalculo { get; set; }
    }
}
