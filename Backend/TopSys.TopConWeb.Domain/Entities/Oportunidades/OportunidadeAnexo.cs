using System;

namespace TopSys.TopConWeb.Domain.Entities.Oportunidades
{
    public class OportunidadeAnexo
    {

        public int Usina { get; set; }
        public int AnoOportunidade { get; set; }
        public int NumeroOportunidade { get; set; }

        public int Interveniente { get; set; }
        public string Nome { get; set; }
        public DateTime DataHora { get; set; }

    }
}
