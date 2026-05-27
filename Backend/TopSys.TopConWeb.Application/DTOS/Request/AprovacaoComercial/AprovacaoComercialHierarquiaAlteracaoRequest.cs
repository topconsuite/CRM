using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaAlteracaoRequest
    {
        public Guid Id { get; set; }
        public Guid AprovacaoComercialUsinaId { get; set; }

        public string Titulo { get; set; }
        public int NivelAutoridade { get; set; }
        public int QuantidadeAprovacoesNecessarias { get; set; }

        public bool AprovacaoObrigatoria { get; set; }

    }
}
