using System;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class IntervenienteAnexo
    {
        public int IntervenienteCodigo { get; set; }
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public DateTime DataHora { get; set; }
        public string Arquivo { get; set; }
        public int AnoChamada { get; set; }
        public int NumeroChamada { get; set; }
        public virtual OportunidadeAnexo OportunidadeAnexo { get; set; }
    }
}
