using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Visita
{
    public class VisitaHistoricoAdicionarRequest
    {

        public int Usina { get; set; }
        public int NumeroVisita { get; set; }
        public int AnoVisita { get; set; }

        public EVisitaHistoricoTipo TipoHistorico { get; set; }
        public string Descricao { get; set; }

        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }

        public DateTime? DataRetorno { get; set; }
        public TimeSpan? HoraRetorno { get; set; }


    }
}
