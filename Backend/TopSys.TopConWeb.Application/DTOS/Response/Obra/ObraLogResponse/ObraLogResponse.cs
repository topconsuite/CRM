using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraLogResponse
{
    public class ObraLogResponse
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public DateTime DataHora { get; set; }

        public string Usuario { get; set; }

        public string Evento { get; set; }

        public int AnoChamada { get; set; }

        public int NumChamada { get; set; }

        public int Sequencia { get; set; }

        public string Complemento { get; set; }

        public string Observacao { get; set; }
    }
}
