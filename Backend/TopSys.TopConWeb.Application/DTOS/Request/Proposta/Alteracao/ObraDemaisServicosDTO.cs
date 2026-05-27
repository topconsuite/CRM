using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class ObraDemaisServicosDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int Sequencia { get; set; }

        public int Codigo { get; set; }

        public UsinaDTO UsinaEntrega { get; set; }

        public MercadoriaDTO Mercadoria { get; set; }

        public UnidadeDTO Unidade { get; set; }

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
