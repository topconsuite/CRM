using System;
using System.Xml.Linq;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceberPublica
{
    public class TituloContasAReceberPagedRequest
    {
        public DateTime? EmissionDate { get; set; }
        public DateTime? OperationDate { get; set; }
        public int DocumentType { get; set; }
        public int Client { get; set; }

        public int? Limit { get; set; } = 10;
        private int? _pagina;

        public int? Page
        {
            get => _pagina ?? 0;
            set
            {
                PaginaSemOffset = value.HasValue ? value : null;
                _pagina = value.HasValue ? (value - 1) * Limit : null;
            }
        }

        public int? PaginaSemOffset { get; set; }
    }
}
