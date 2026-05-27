using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada
{
    public class AprovacaoComercialUsina
    {
        public Guid Id { get; set; }
        public int UsinaId { get; set; }
        public virtual Usina Usina { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual List<AprovacaoComercialHierarquia> Hierarquias { get; set; }
        public EAprovacaoComercialUsinaFluxoAprovacao FluxoAprovacao { get; set; }

    }
}
