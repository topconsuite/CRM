using System;
using Topsys.TopConWeb.SharedKernel.Filters;

namespace TopSys.TopConWeb.Infra.Reports.FilterClasses
{
    public class RelatorioProducaoFilter : IFilter
    {
        public int? Usina { get; set; }

        public int? Cliente { get; set; }

        public int? Vendedor { get; set; }
        
        public int? VendedorPadrinho { get; set; }

        public string VendedorIn { get; set; }

        public DateTime? DataDe { get; set; }

        public DateTime? DataAte { get; set; }

        public string AnoContrato { get; set; }

        public string NumContrato { get; set; }


        public int? Segmentacao { get; set; }
        public int? ContratoFinalidade { get; set; }
        public int? ViaCaptacao { get; set; }
        public int? GrupoEconomico { get; set; }
    }
}
