using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Opportunity
{
    public class UsinaDTO
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Sigla { get; set; }

        public int? FilialCodigo { get; set; } = 0;
        public int PrazoPesagem { get; set; }
        public float PorcentagemRetornoObra { get; set; }
        public int TempoBtNaObra { get; set; }
        public int TempoBtAteAObra { get; set; }
    }
}
