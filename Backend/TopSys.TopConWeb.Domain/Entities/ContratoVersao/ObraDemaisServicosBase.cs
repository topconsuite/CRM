using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using static TopSys.TopConWeb.SharedKernel.Helpers.StringHelper;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraDemaisServicosBase <TObra, TContrato, TObraTraco, TObraBomba> : IObraDemaisServicos<TObra, TContrato, TObraTraco, TObraBomba>
        where TContrato : IHasInterveniente, IHasVendedor, IContrato
        where TObraTraco : ObraTracoBase<TObra>
        where TObraBomba : ObraBombaBase<TObra>
        where TObra : IObra<TObra, TContrato, TObraTraco, TObraBomba>

    {
        public int UsinaCodigo { get; set; }

        public int ObraNumero { get; set; }

        public int Sequencia { get; set; }

        public int Codigo { get; set; }

        public int UsinaEntregaCodigo { get; set; }
        public virtual Usina UsinaEntrega { get; set; }

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

        public float PrecoProposto { get; set; }

        public float Quantidade { get; set; }

        public float CalcularValorTotal(IObra<TObra, TContrato, TObraTraco, TObraBomba> obra)
        {
            switch (FrequenciaDeCobranca)
            {
                case EFrequenciaDeCobranca.Bombeamento:
                    return (float)Math.Round((obra?.ObraBombas?.Count ?? 0f) > 0 ? PrecoProposto * Quantidade : 0f, 2);
                case EFrequenciaDeCobranca.M3Bombeado:
                    return (float)Math.Round((obra?.ObraBombas?.Count ?? 0f) > 0 ? PrecoProposto * Quantidade * (obra?.ObraTracos?.Sum(t => t.M3Quantidade) ?? obra?.VolumeEstimado ?? 0f) : 0f, 2);
                case EFrequenciaDeCobranca.M3:
                    return (float)Math.Round(PrecoProposto * Quantidade * (obra?.ObraTracos?.Sum(t => t.M3Quantidade) ?? obra?.VolumeEstimado ?? 0f), 2);
                case EFrequenciaDeCobranca.Programacao:
                    return (float)Math.Round(PrecoProposto * Quantidade, 2);
                case EFrequenciaDeCobranca.Remessa:
                    float volumePorCarga = (obra?.VolumePorCarga ?? 0f) > 0 ? obra.VolumePorCarga : 8f;
                    var viagens = Math.Ceiling((obra?.ObraTracos?.Sum(t => t.M3Quantidade) ?? obra?.VolumeEstimado ?? 0f) / volumePorCarga);
                    return (float)Math.Round(PrecoProposto * Quantidade * viagens, 2);
                case EFrequenciaDeCobranca.Contrato:
                    return (float)Math.Round(PrecoProposto * Quantidade, 2);
                default:
                    return (float)Math.Round(PrecoProposto * Quantidade, 2);
            }
        }
    }
}
