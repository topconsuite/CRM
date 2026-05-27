namespace TopSys.TopConWeb.Application.DTOS.Response.TipoDocumento
{
    public class TipoDocumentoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public TipoDocumentoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}