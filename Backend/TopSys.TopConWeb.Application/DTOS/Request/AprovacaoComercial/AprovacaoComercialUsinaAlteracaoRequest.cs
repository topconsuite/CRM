using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialUsinaAlteracaoRequest
    {

        public Guid Id { get; set; }
        public int UsinaId { get; set; }
        public bool Ativo { get; set; }
        public EAprovacaoComercialUsinaFluxoAprovacao FluxoAprovacao { get; set; }

    }
}
