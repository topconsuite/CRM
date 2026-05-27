using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.MunicipioTributacao
{
    public class MunicipioTributacaoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public MunicipioTributacaoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}