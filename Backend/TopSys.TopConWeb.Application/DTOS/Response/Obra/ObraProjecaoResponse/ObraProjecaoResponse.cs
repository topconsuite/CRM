using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraProjecaoResponse
{
    public class ObraProjecaoResponse
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int AnoChamada { get; set; }

        public int NoChamada { get; set; }

        public DateTime Periodo { get; set; }

        public float Volume { get; set; }

        public float Saldo { get; set; }
    }
}