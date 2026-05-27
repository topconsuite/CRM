using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
   public class ContratoDTO
    {
        public DateTime? DataEncerramento { get; set; }

        public string NumeroContratoAnterior { get; set; }
        public DateTime? FimVigencia { get; set; }
    }
}