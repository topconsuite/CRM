using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Alteracao
{
    public class DemaisServicosAlteracaoRequest
    {
        public int Codigo { get; set; }

        public int UsinaCodigo { get; set; }

        public string MercadoriaCodigo { get; set; }

        public string UnidadeSigla { get; set; }

        public int NumeroDeCasasDecimais { get; set; }

        public float PrecoSugerido { get; set; }

        public float PrecoMinimo { get; set; }

        public EFrequenciaDeCobranca FrequenciaDeCobranca { get; set; }

        public EFormaDeCobrancaDemaisServicos FormaDeCobranca { get; set; }

        public bool AtualizaEstoque { get; set; }
    }
}
