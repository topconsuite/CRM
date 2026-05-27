using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse
{
    public class ObraBombaDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public int BombaTipoCodigo { get; set; }

        public CadastroGeralDTO BombaTipo { get; set; }

        public int M3TabelaAte { get; set; }

        public float TaxaMinimaPrecoTabela { get; set; }

        public float M3PrecoTabela { get; set; }

        public int M3PropostoAte { get; set; }

        public float TaxaMinimaPrecoProposto { get; set; }

        public float M3PrecoProposto { get; set; }

        public float DescontoPercentual { get; set; }

        public string DescontoSolicitante { get; set; }
   
        public int StatusAprovacao { get; set; }

        public string Justificativa { get; set; }
        public float TotalImpostos { get; set; }
        public float Ebitda { get; set; }
        public string Ativo { get; set; }
        public Boolean Inativo { get; set; }

    }
}
