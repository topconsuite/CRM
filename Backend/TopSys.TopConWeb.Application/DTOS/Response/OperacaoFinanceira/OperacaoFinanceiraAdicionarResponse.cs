namespace TopSys.TopConWeb.Application.DTOS.Response.OperacaoFinanceira
{
    public class OperacaoFinanceiraAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public OperacaoFinanceiraAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}