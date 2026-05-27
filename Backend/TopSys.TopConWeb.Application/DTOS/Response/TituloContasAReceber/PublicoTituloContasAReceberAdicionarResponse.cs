using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber
{
    public class PublicoTituloContasAReceberAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public PublicoTituloContasAReceberAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}
