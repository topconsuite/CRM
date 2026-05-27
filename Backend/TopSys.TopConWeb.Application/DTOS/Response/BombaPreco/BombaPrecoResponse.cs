using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.BombaPreco
{
    public class BombaPrecoResponse
    {
        public virtual UsinaDTO Usina { get; set; }
        
        public CadastroGeralDTO BombaTipo { get; set; }

        public DateTime DataInicioVigencia { get; set; }

        public int M3Ate { get; set; }

        public float TaxaMinimaPreco { get; set; }

        public float M3Preco { get; set; }

        public int M3AteValorMinimo { get; set; }

        public float TaxaMinimaPrecoPercentualDescontoMaximo { get; set; }
        public float TaxaMinimaPrecoValorMinimo { get; set; }

        public float M3PrecoPercentualDescontoMaximo { get; set; }
        public float M3PrecoValorMinimo { get; set; }

        public EBombaM3CalculoTipo TipoCalculo { get; set; }

        public float HoraAte { get; set; }

        public float HoraTaxaMinimaPreco { get; set; }

        public float HoraPreco { get; set; }

        public float HoraAteValorMinimo { get; set; }

        public float HoraTaxaMinimaPrecoValorMinimo { get; set; }

        public float HoraPrecoValorMinimo { get; set; }

        public EBombaHoraCalculoTipo HoraTipoCalculo { get; set; }
    }
}
