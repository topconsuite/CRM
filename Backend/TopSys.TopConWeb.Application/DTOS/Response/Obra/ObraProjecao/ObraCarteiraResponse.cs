using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecaoDTO;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraProjecao
{
    public class ObraCarteiraResponse
    {

        public int UsinaCodigo { get; set; }

        public int UsinaEntregaCodigo { get; set;  }
        
        public UsinaDTO UsinaEntrega { get; set; }

        public int Numero { get; set; }

        public string Nome { get; set; }

        public int ObraCodigo { get; set; }

        public int? AnoChamada { get; set; } = 0;
        public int? NumChamada { get; set; } = 0;

        public int? AnoContrato { get; set; } = 0;
        public int? NumContrato { get; set; } = 0;

        public DateTime Periodo { get; set; }

        public float Volume { get; set; }

        public float Saldo { get; set; }

        public ContratoDTO Contrato { get; set; }

        public virtual EStatusProjecao StatusProjecao { get; set; }

        public ICollection<ObraProjecaoDTO> ObraProjecao { get; set; }

    }
}
