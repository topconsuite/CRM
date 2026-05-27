using System;

namespace TopSys.TopConWeb.Domain.Entities.Oportunidades
{
    public class OportunidadeLog
    {

        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public string Tipo { get; set; }
        public DateTime DataHoraEvento { get; set; }
        public string Usuario { get; set; }
        public string Evento { get; set; }
        public string Complemento { get; set; }

        public virtual Oportunidade Oportunidade { get; set; }
    }
}
