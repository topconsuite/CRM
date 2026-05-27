using System;

namespace TopSys.TopConWeb.Domain.Entities.MotivoPerdas
{
    public class MotivoPerdaLog
    {
        public string Tipo { get; set; }

        public DateTime DataHoraEvento { get; set; }

        public string Usuario { get; set; }

        public string Evento { get; set; }

        public string Complemento { get; set; }
    }
}
