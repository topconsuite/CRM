using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.SituacaoFinanceira
{
    public class SituacaoFinanceiraAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public SituacaoFinanceiraAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}