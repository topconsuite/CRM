using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceberPublica
{
    public class TitulosContasAReceberUpdateRequest
    {
        public int Company { get; set; }
        public int DocumentType { get; set; }
        public string DocumentSerie { get; set; }
        public long DocumentNumber { get; set; }
        public int Sequence { get; set; }
        public int Splitting { get; set; }

        public AccountsReceivable accountsReceivable;

    }

    
}
