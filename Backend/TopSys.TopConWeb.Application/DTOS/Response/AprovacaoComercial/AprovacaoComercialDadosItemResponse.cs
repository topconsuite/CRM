using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialDadosResponse
    {

        public List<AprovacaoComercialDadosItemResponse> Tracos { get; set; }
        public List<AprovacaoComercialDadosItemResponse> Bombas { get; set; }
        public List<AprovacaoComercialDadosItemResponse> Volumes { get; set; }
        public List<AprovacaoComercialDadosItemResponse> CondicoesPagamento { get; set; }

    }

    public class AprovacaoComercialDadosItemResponse
    {

        public int NivelAutoridade { get; set; }
        public string NivelDescricao { get; set; }

        public int Sequencia { get; set; }

        public int QuantidadeNotificacoes { get; set; }
        public int QuantidadeNotificacoesAprovadas { get; set; }
        public int QuantidadeNotificacoesReprovadas { get; set; }
        public EAprovacaoComercialPendenteStatus Status { get; set; }
        public string Aprovadores { get; set; }
        public string Reprovadores { get; set; }

    }
}
