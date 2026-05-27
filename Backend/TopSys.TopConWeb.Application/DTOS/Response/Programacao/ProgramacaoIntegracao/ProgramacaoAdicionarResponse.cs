namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoIntegracao
{
    public class ProgramacaoAdicionarResponse
    {
        public int TotalRecordsInserted { get; set; }

        public ProgramacaoAdicionarResponse(int totalRecordsInserted)
        {
            TotalRecordsInserted = totalRecordsInserted;
        }
    }
}
