namespace TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento
{
    public class CondicaoDePagamentoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public CondicaoDePagamentoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}