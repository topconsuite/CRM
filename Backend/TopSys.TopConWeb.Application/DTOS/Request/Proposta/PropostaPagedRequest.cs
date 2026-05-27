using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Paged;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta
{
    public class PropostaPagedRequest : PagedRequest
    {
        public int FiltroStatusProposta { get; set; }
        public int FiltroExibicaoContratos { get; set; }
        public bool FiltroDivergenciaCarteira { get; set; }
        public EStatusClicksignDocumento? StatusClicksignDocumento { get; set; }
    }
}
