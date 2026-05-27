using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.BombaPreco
{
    public class BombaPrecoTerceiroResponse
    {
        public virtual IntervenienteDTO Bombista { get; set; }
        
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
    }
}
