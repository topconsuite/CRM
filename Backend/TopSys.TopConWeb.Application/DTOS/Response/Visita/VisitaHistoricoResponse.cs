using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Visita
{
    public class VisitaHistoricoResponse
    {

        public Guid Id { get; set; }

        public int Usina { get; set; }
        public int NumeroVisita { get; set; }
        public int AnoVisita { get; set; }

        public EVisitaHistoricoTipo TipoHistorico { get; set; }
        public string Descricao { get; set; }

        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }

        public DateTime? DataRetorno { get; set; } 
        public TimeSpan? HoraRetorno { get; set; }

        public string IdCadastro { get; set; }
        

    }
}
