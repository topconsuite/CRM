using TopSys.TopConWeb.Application.DTOS.Response.Segmentacao;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoSimplesResponse
{
    public class PropostaDTO
    {
        public int Usina { get; set; }

        public int Ano { get; set; }

        public int Numero { get; set; }

        public int? IntervenienteCodigo { get; set; } = 0;

        public string IntervenienteRazao { get; set; }

        public string Contato { get; set; }

        public EPropostaStatusCliente StatusProposta { get; set; }

        public EObraStatusComercial StatusComercial { get; set; }

        public CondicaoPagamentoDTO ObraCondicaoPagamento { get; set; }

        public ObraDTO Obra { get; set; }

        public SegmentacaoResponse Segmento { get; set; }
    }
}
