using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TipoCobranca
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string Forma { get; set; }

        public int? PortadorCodigo { get; set; } = 0;
        public Portador Portador { get; set; }

        public int Situacao { get; set; }

        public string Fixo { get; set; }

        public string Aprovacao { get; set; }


        public bool TipoCobrancaCartao()
        {
            return Forma == "CD" || Forma == "CC";
        }
        public bool TipoCobrancaBoleto()
        {
            return Forma == "BE";
        }

        public bool TipoCobrancaCheque()
        {
            return Forma == "CH" || Forma == "CP";
        }
    }
}
