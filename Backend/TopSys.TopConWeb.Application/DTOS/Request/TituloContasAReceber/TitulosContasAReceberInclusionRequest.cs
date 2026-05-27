using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceberPublica
{
    public class TitulosContasAReceberInclusionRequest
    {
        public AccountsReceivable accountsReceivable { get; set; }
    }

    
}
