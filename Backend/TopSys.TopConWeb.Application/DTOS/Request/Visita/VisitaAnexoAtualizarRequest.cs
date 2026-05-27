using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.Visita
{
    public class VisitaAnexoAtualizarRequest
    {
        public Guid Id { get; set; }
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public string Descricao { get; set; }
        public string Nome { get; set; }
        public DateTime DataHora { get; set; }

    }
}
