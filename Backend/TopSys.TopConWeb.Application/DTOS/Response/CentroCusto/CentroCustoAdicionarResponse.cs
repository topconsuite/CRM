using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.CentroCusto
{
    public class CentroCustoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public CentroCustoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}