using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.TipoDeCobranca
{
    public class TipoDeCobrancaAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public TipoDeCobrancaAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}