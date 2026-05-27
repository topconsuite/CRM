using System;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaCondicaoInsercaoRequest
    {
        public Guid Id { get; set; }
        public double ValorDe { get; set; }
        public double ValorAte { get; set; }
        public double PercentualDe { get; set; }
        public double PercentualAte { get; set; }

        public Guid TipoPessoaId { get; set; }


        public Guid AprovacaoComercialHierarquiaId { get; set; }


        public string Valor { get; set; }

        public string Condicao { get; set; }

    }
}
