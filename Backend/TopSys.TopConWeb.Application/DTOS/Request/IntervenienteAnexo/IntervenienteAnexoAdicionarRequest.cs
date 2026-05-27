namespace TopSys.TopConWeb.Application.DTOS.Request.IntervenienteAnexo
{
    public class IntervenienteAnexoAdicionarRequest
    {
        public int IntervenienteCodigo { get; set; }
        public string Nome { get; set; }
        public string Arquivo { get; set; }
        public int AnoChamada { get; set; }
        public int NumeroChamada { get; set; }

        public int AnoOportunidade { get; set; }
        public int NumeroOportunidade { get; set; }
    }
}
