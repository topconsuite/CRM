using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Funcionario
{
    public class FuncionarioAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public FuncionarioAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }

    }
}
