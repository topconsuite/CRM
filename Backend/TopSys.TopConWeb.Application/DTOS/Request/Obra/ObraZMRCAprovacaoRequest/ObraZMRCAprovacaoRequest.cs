using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraZMRCAprovacaoRequest
{
    public class ObraZMRCAprovacaoRequest
    {
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public ContratoDTO Contrato { get; set; }
    }
}




