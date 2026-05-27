using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral;

namespace TopSys.TopConWeb.Application.DTOS.Response.CadastroGeral
{
    public class CadastroGeralIntegracaoAdicionarResponses
    {
        public int TotalRecordsInserted { get; set; }

        public CadastroGeralIntegracaoAdicionarResponses(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}
