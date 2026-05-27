using System;

namespace TopSys.TopConWeb.Domain.Entities.Visitas
{
    public class VisitaAnexo
    {
        public Guid Id { get; set; }
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public DateTime DataHora { get; set; }
        public string Arquivo { get; set; }
        
    }
}
