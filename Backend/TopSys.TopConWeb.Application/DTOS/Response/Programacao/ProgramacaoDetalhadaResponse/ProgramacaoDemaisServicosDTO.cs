using TopSys.TopConWeb.Application.DTOS.Response.Mercadoria;
using TopSys.TopConWeb.Application.DTOS.Response.Unidade;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoDetalhadaResponse
{
    public class ProgramacaoDemaisServicosDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int ProgramacaoSequencia { get; set; }

        public int Sequencia { get; set; }

        public float Quantidade { get; set; }
    }
}
