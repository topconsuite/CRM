using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Equipamento;

namespace TopSys.TopConWeb.Application.DTOS.Response.Equipamento
{
    public class EquipamentoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public EquipamentoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }

    }
}
