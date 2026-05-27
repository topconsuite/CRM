using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialCondicaoPagamentoResponse
    {

        public Guid TipoPessoaId { get; set; }
        public Guid AprovacaoComercialHierarquiaId { get; set; }
        public int SegmentacaoId { get; set; }

        public float MediaDiasDe { get; set; }
        public float MediaDiasAte { get; set; }

    }

}
