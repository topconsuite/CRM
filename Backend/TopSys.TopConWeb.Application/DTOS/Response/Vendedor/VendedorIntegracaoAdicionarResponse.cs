using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Vendedor;

namespace TopSys.TopConWeb.Application.DTOS.Response.Vendedor
{
    public class VendedorIntegracaoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public VendedorIntegracaoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}
