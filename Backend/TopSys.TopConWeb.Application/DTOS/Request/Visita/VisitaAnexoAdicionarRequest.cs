namespace TopSys.TopConWeb.Application.DTOS.Request.Visita
{
    public class VisitaAnexoAdicionarRequest
    {

        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public string Nome { get; set; }
        public string Arquivo { get; set; }

    }
}
