using System;
using TopSys.TopConWeb.Domain.Enums;
using static TopSys.TopConWeb.SharedKernel.Helpers.StringHelper;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class DemaisServicos
    {
        public int Codigo { get; set; }

        public int UsinaCodigo { get; set; }
        public virtual Usina Usina { get; set; }

        public string MercadoriaCodigo { get; set; }
        public virtual Mercadoria Mercadoria { get; set; }

        public string UnidadeSigla { get; set; }
        public virtual Unidade Unidade { get; set; }

        public int NumeroDeCasasDecimais { get; set; }

        public float PrecoSugerido { get; set; }

        public float PrecoMinimo { get; set; }

       
        public string FrequenciaDeCobrancaString
        {
            get { return FrequenciaDeCobranca.ToString(); }
            private set { FrequenciaDeCobranca = EnumExtensions.ParseEnum<EFrequenciaDeCobranca>(value); }
        } 
        public EFrequenciaDeCobranca FrequenciaDeCobranca { get; set; }


        public string FormaDeCobrancaString
        {
            get { return FormaDeCobranca.ToString(); }
            private set { FormaDeCobranca = EnumExtensions.ParseEnum<EFormaDeCobrancaDemaisServicos>(value); }
        }
        public EFormaDeCobrancaDemaisServicos FormaDeCobranca { get; set; }

        public bool AtualizaEstoque { get; set; }

    }
}
