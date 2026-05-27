using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.CartaoBandeira
{
    public class CartaoBandeiraResponse
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string TipoIntegracao { get; set; }

        public virtual IntervenienteDTO Interveniente { get; set; }
        
        public virtual PortadorDTO Portador { get; set; }
    }
}
