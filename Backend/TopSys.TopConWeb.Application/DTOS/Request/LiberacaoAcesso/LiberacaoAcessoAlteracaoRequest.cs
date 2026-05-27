using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.LiberacaoAcesso
{
    public class LiberacaoAcessoAlteracaoRequest
    {
        public int Codigo { get; set; }

        public string Usuario { get; set; }

        public int Grupo { get; set; }

        public string TipoLiberacao { get; set; }

        public string DiaSemana { get; set; }

        public int Turno { get; set; }

        public TimeSpan HoraEntrada { get; set; }

        public TimeSpan HoraSaida { get; set; }

        public Boolean Bloquear { get; set; }

        public DateTime? CriadoEm { get; set; }

        public DateTime? AtualizadoEm { get; set; }
    }
}
