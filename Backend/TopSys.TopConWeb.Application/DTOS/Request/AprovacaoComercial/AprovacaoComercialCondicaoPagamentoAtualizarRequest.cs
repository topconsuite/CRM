using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialCondicaoPagamentoAtualizarRequest
    {

        public Guid TipoPessoaId { get; set; }
        public Guid AprovacaoComercialHierarquiaId { get; set; }
        public int SegmentacaoId { get; set; }

        public float MediaDiasDe { get; set; }
        public float MediaDiasAte { get; set; }

    }
}
