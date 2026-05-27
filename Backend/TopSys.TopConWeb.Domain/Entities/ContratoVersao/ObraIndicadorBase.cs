using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraIndicadorBase
    {

        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }

        public int IntervenienteCodigo { get; set; }
        public virtual Interveniente Interveniente { get; set; }

        public int VendedorCodigo { get; set; }
        public virtual Vendedor Vendedor { get; set; }

    }
}
