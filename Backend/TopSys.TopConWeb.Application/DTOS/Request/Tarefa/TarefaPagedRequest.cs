using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Paged;

namespace TopSys.TopConWeb.Application.DTOS.Request.Tarefa
{
    public class TarefaPagedRequest : PagedRequest
    {
        public bool FiltroTarefasAtrasadas { get; set; }  = false;
        public string FiltroHorarioDe { get; set; }
        public string FiltroHorarioAte { get; set; }
        public string FiltroUsuarios { get; set; }
    }
}
