using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.BoletoExterno
{
    public class BoletoExternoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public BoletoExternoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}