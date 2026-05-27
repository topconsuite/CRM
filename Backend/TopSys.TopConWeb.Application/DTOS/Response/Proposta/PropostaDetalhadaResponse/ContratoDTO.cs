using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
   public class ContratoDTO
    {
        public ICollection<ContratoTracoReajusteDTO> ContratoTracoReajustes { get; set; }

        public ICollection<ContratoBombaReajusteDTO> ContratoBombaReajustes { get; set; }

        public DateTime? DataEncerramento { get; set; }

        public string NumeroContratoAnterior { get; set; }

        public DateTime? DataContrato { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }
    }
}
