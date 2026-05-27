using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.Lead
{
    public class LeadInteracaoAdicionarRequest
    {
        public int Usina { get; set; }
        public int NumeroLead { get; set; }
        public int AnoLead { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
        public DateTime DataRetorno { get; set; }
        public DateTime HoraRetorno { get; set; }
    }
}
