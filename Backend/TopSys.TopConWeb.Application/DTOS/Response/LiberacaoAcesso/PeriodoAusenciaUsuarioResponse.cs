using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.LiberacaoAcesso
{
    public class PeriodoAusenciaUsuarioResponse
    {
        public int Codigo { get; set; }

        public string Usuario { get; set; }

        public string TipoLiberacao { get; set; }

        public string TipoAusencia { get; set; }

        public DateTime InicioPeriodo { get; set; }

        public DateTime FimPeriodo { get; set; }

        public DateTime? CriadoEm { get; set; }

        public DateTime? AtualizadoEm { get; set; }
    }
}
