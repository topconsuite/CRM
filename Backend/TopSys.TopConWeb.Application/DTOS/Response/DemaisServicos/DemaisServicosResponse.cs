using TopSys.TopConWeb.Application.DTOS.Response.Mercadoria;
using TopSys.TopConWeb.Application.DTOS.Response.Unidade;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.DemaisServicos
{
    public class DemaisServicosResponse
    {
        public int Codigo { get; set; }

        public virtual UsinaResponse Usina { get; set; }

        public virtual MercadoriaResponse Mercadoria { get; set; }

        public virtual UnidadeResponse Unidade { get; set; }

        public int NumeroDeCasasDecimais { get; set; }

        public float PrecoSugerido { get; set; }

        public float PrecoMinimo { get; set; }

        public EFrequenciaDeCobranca FrequenciaDeCobranca { get; set; }

        public EFormaDeCobrancaDemaisServicos FormaDeCobranca { get; set; }

        public bool AtualizaEstoque { get; set; }
    }
}
