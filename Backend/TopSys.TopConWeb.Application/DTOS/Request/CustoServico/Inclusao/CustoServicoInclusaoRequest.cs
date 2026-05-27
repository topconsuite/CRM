using System;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;

namespace TopSys.TopConWeb.Application.DTOS.Request.CustoServico.Inclusao
{
    public class CustoServicoInclusaoRequest
    {
        public int UsinaCodigo { get; set; }
        public int UsinaTabelaPreco { get; set; }
        public DateTime DataInicioVigencia { get; set; }
        public float CustoMedioServico { get; set; }
        public float CustoMedioHoraTransporte { get; set; }
        public float CustoMedioProducao { get; set; }
        public float CustoMedioBombagem { get; set; }
        public float CustoMedioLaboratorio { get; set; }
        public float CustoMedioImpostos { get; set; }
        public float CustoMedioLubrificantes { get; set; }
        public float CustoMedioManutencao { get; set; }
        public float CustoMedioMaoDeObra { get; set; }
        public float CustoMedioCombustivel { get; set; }
        public float OutrosCustosMateriais { get; set; }
        public float Lucro { get; set; }
        public int FormaMedidaAditivo { get; set; }
        public float CustoMedioComercial { get; set; }
        public float CustoMedioAdministrativo { get; set; }
        public float Markup { get; set; }
        public float ImpostoEstadual { get; set; }
        public float ImpostoFederal { get; set; }
    }
}
