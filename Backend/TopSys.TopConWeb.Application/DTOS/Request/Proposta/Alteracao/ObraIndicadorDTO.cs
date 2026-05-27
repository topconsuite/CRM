using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class ObraIndicadorDTO
    {

        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }

        public int IntervenienteCodigo { get; set; }

        public int VendedorCodigo { get; set; }

    }
}
