using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.IntervenienteAnexo
{
    public class IntervenienteAnexoResponse
    {
        public int IntervenienteCodigo { get; set; }
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public DateTime DataHora { get; set; }
        public int AnoChamada { get; set; }
        public int NumeroChamada { get; set; }
    }
}
