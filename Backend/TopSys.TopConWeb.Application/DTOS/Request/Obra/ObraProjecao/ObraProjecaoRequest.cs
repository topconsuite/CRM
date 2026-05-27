using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecao
{
    public class ObraProjecaoRequest
    {
        public int Usina { get; set; }
        public int NoObra { get; set; }
        public int AnoChamada { get; set; }
        public int NoChamada { get; set; }
        public DateTime Periodo { get; set; }
        public float Volume { get; set; }
        public float Saldo { get; set; }
        public float VolumeAnterior { get; set; }
        public DateTime PeriodoAnterior { get; set; }
    }
}
