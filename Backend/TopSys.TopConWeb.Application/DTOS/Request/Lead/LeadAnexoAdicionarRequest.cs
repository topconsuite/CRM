namespace TopSys.TopConWeb.Application.DTOS.Request.Lead
{
    public class LeadAnexoAdicionarRequest
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string Arquivo { get; set; }
    }
}
