using System;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialCondicaoPagamentoItemResponse
    {

        public int CondicaoPagamentoCodigo { get; set; }
        public string CondicaoPagamentoNome { get; set; }

        public Guid TipoPessoaId { get; set; }
        public Guid AprovacaoComercialHierarquiaId { get; set; }

        public bool NecessitaAprovacao { get; set; } = false;
        public string AprovacaoComercialHierarquiaCondicaoPagamentoId { get; set; }

    }

}
