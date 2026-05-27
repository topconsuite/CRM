using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse
{
    public class ContratoDTO
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public EStatusClicksignDocumento StatusClicksignDocumento { get; set; }
        public string NumeroContratoAnterior { get; set; }
        public DateTime? DataContrato { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }
    }
}
