using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialCondicaoPagamentoRemoverLoteRequest
    {

        public Guid TipoPessoaId { get; set; }
        public Guid AprovacaoComercialHierarquiaId { get; set; }
        public int SegmentacaoId { get; set; }

    }
}
