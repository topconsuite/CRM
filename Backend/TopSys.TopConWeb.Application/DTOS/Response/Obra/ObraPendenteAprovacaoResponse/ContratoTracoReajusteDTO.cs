using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse
{
    public class ContratoTracoReajusteDTO
    {
        public int ObraTracoSequencia { get; set; }

        public int ContratoAno { get; set; }

        public int ContratoNumero { get; set; }

        public UsoDTO Uso { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public PedraDTO Pedra { get; set; }

        public SlumpDTO Slump { get; set; }

        public DateTime DataVigencia { get; set; }

        public float PrecoRecalculado { get; set; }

        public float ValorServicoRecalculado { get; set; }

        public float CustoRecalculado { get; set; }

        public DateTime? DataConfirmacao { get; set; }

    }
}
