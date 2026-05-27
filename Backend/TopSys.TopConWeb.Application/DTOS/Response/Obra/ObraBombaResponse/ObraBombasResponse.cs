using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracosResponse;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraBombaResponse
{
    public class ObraBombasResponse
    {
        public int UsinaCodigo { get; set; }

        public int Numero { get; set; }

        public int AnoChamada { get; set; }

        public int NumChamada { get; set; }

        public int AnoContrato { get; set; }

        public int NumContrato { get; set; }

        public ContratoDTO Contrato { get; set; }
        public ICollection<ObraBombaDTO> ObraBombas { get; set; }
    } 
}
