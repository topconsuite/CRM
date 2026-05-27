using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Banco
{
    public class BancoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public BancoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}
