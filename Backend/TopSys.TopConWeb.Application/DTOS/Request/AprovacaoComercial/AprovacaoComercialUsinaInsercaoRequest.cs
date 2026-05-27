using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialUsinaInsercaoRequest
    {
        public int UsinaId { get; set; }
        public bool Ativo { get; set; }
        public EAprovacaoComercialUsinaFluxoAprovacao FluxoAprovacao { get; set; }

    }
}
