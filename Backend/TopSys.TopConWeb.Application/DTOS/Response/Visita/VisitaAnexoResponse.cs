using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Visita
{
    public class VisitaAnexoResponse
    {
        public Guid Id { get; set; }
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public DateTime DataHora { get; set; }

    }
}
