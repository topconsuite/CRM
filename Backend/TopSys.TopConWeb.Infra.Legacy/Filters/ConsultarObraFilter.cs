using System;
using Topsys.TopConWeb.SharedKernel.Filters;

namespace TopSys.TopConWeb.Infra.Legacy.Filters
{
    public class ConsultarObraFilter : IFilter
    {
        public DateTime? ProgramacaoDataHoraDe { get; set; }
        public DateTime? ProgramacaoDataHoraAte { get; set; }
        public string CpfCnpj { get; set; }
        public int? Cliente { get; set; }
        public int? Vendedor { get; set; }
        public DateTime? PeriodoDe { get; set; }
        public DateTime? PeriodoAte { get; set; }
        public bool ConsiderarEncerrados { get; set; }
        public string UsinaEntregaIn { get; set; }
        public string AnalistaIn { get; set; }
        public string TipoCobrancaIn { get; set; }
        public string PortadorIn { get; set; }
        public string BandeiraIn { get; set; }

        public string StatusCadastroIn { get; set; }
        public string StatusComercialIn { get; set; }
        public string StatusEngenhariaIn { get; set; }
        public string StatusFinanceiroIn { get; set; }
        public string StatusGeracaoContratoIn { get; set; }
        public string StatusClicksignDocumentoIn { get; set; }

        public bool AnalisarLimiteCredito { get; set; }
        public bool AnaliseLimiteConsiderarPrevisao { get; set; }

        public int? AnoContrato { get; set; }

        public int? NumeroContrato { get; set; }

        public int? AnoProposta { get; set; }

        public int? NumeroProposta { get; set; }
        public string VendedoresPermitidos { get; set; }

        public int? Segmentacao { get; set; }
        public int? ContratoFinalidade { get; set; }

        public bool AguardandoOutroNivel { get; set; }

        public int? GrupoEconomico { get; set; }
    }
}
