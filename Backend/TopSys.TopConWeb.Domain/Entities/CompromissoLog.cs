using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class CompromissoLog
    {
        public int CodigoCompromisso { get; set; }

        public DateTime DataHoraEvento { get; set; }

        public string Usuario { get; set; }

        public string Descricao { get; set; }

        public int Usina { get; set; }

        public int AnoVisita { get; set; }

        public int NumeroVisita { get; set; }

        public int AnoLead { get; set; }

        public int NumeroLead { get; set; }

        public int AnoOportunidade { get; set; }

        public int NumeroOportunidade { get; set; }

        public string Evento { get; set; }

        public string Complemento { get; set; }
    }
}
