using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.PortadorCobranca
{
    public class PortadorCobrancaAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public PortadorCobrancaAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}