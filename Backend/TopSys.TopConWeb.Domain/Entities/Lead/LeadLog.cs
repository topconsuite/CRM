using System;

namespace TopSys.TopConWeb.Domain.Entities.Lead
{
    public class LeadLog
    {
        public int Usina { get; set; }
        public int AnoLead { get; set; }
        public int NumeroLead { get; set; }
        public int Sequencia { get; set; }
        public string Tipo { get; set; }
        public DateTime DataHoraEvento { get; set; }
        public string Usuario { get; set; }
        public string Evento { get; set; }
        public string Complemento { get; set; }
    }
}
