using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.Lead
{
    public class LeadAnexoAtualizarRequest
    {
        public Guid Id { get; set; }
        public int Usina { get; set; }
        public int AnoLead { get; set; }
        public int NumeroLead { get; set; }
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public DateTime DataHora { get; set; }
    }
}
