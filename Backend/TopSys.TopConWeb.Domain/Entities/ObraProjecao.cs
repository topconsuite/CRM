using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ObraProjecao
    {
        public int Usina { get; set; }

        public int NoObra { get; set; }

        public int AnoChamada { get; set; }

        public int NoChamada { get; set; }

        public DateTime Periodo { get; set; }

        public float Volume { get; set; }

        public float Saldo { get; set; }

        public Obra Obra { get; set; }
    }
}
