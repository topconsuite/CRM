using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraTributacaoMunicipalBase
    {
        public int ObraUsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int? ContratoAno { get; set; } = 0;
        public int? ContratoNumero { get; set; } = 0;

        public int UsinaEntregaCodigo { get; set; }

        public string CodigoObraPrefeitura { get; set; }

        public string ObraCCM { get; set; }

        public int TributacaoISS { get; set; }
        public string RetencaoISS { get; set; }
    }
}
