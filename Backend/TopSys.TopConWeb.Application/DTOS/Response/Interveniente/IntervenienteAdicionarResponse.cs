using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Interveniente
{
    public class IntervenienteAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public IntervenienteAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }   
    }
}
