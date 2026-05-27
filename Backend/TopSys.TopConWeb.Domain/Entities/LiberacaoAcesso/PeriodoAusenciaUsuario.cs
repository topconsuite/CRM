using System;

namespace TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso
{
    public class PeriodoAusenciaUsuario
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
