using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Paged;

namespace TopSys.TopConWeb.Application.DTOS.Request.Compromisso
{
    public class CompromissoPagedRequest : PagedRequest
    {
        public string filtroHoraInicioDe { get; set; }
        public string filtroHoraInicioAte { get; set; }
        public string filtroHoraFinalDe { get; set; }
        public string filtroHoraFinalAte { get; set; }
        public string FiltroUsuarios { get; set; }
    }
}
