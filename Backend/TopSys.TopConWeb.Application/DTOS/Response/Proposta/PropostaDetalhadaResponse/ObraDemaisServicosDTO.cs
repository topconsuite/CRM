using TopSys.TopConWeb.Application.DTOS.Response.Mercadoria;
using TopSys.TopConWeb.Application.DTOS.Response.Unidade;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaDetalhadaResponse
{
    public class ObraDemaisServicosDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int Sequencia { get; set; }

        public int Codigo { get; set; }

        public virtual UsinaDTO UsinaEntrega { get; set; }

        public virtual MercadoriaResponse Mercadoria { get; set; }

        public virtual UnidadeResponse Unidade { get; set; }

        public int NumeroDeCasasDecimais { get; set; }

        public float PrecoSugerido { get; set; }

        public float PrecoMinimo { get; set; }

        public EFrequenciaDeCobranca FrequenciaDeCobranca { get; set; }

        public EFormaDeCobrancaDemaisServicos FormaDeCobranca { get; set; }

        public bool AtualizaEstoque { get; set; }

        public float PrecoProposto { get; set; }

        public float Quantidade { get; set; }
    }
}
