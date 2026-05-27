using System;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialUsinaResponse
    {

        public Guid Id { get; set; }
        public int UsinaId { get; set; }
        public UsinaResponse Usina { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AprovacaoComercialHierarquiaResponse[] Hierarquias { get; set; }
        public EAprovacaoComercialUsinaFluxoAprovacao FluxoAprovacao { get; set; }

    }
}
