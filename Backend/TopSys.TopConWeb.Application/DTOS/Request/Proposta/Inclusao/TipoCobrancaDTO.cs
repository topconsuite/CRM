using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao
{
    public class TipoCobrancaDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string Forma { get; set; }
        
        public PortadorDTO Portador { get; set; }

        public string Fixo { get; set; }

        public string Aprovacao { get; set; }
    }
}
