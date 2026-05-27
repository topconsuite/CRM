using System;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraTraco
{
    public class CalcularEbitdaObraTraco
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public string Resistencia { get; set; }

        public ObraPendenteAprovacaoRequest.UsoDTO Uso { get; set; }

        public ObraPendenteAprovacaoRequest.PedraDTO Pedra { get; set; }

        public ObraPendenteAprovacaoRequest.SlumpDTO Slump { get; set; }

        public float M3Quantidade { get; set; }

        public float M3PrecoTabela { get; set; }

        public float M3PrecoProposto { get; set; }

        public float M3PrecoAjustado { get; set; }
        public float PrecoReajustadoAtual { get; set; }
        public float PrecoReajustadoAnterior { get; set; }
        public DateTime? DataUltimoReajuste { get; set; }

        public float DescontoPercentual { get; set; }

        public float ValorServico { get; set; }

        public string DescontoSolicitante { get; set; }

        public string AprovacaoVerbal { get; set; }

        public string AprovacaoObservacao { get; set; }

        public int StatusAprovacao { get; set; }

        public string Justificativa { get; set; }
        public float Ebitda { get; set; }
        public float MargemPosTransporte { get; set; }
        public float IssDedutivel { get; set; }
        public float ImpostoAplicadoEstadual { get; set; }
        public float ImpostoAplicadoFederal { get; set; }
        public float CustoBombagem { get; set; }
        public float TotalImpostos { get; set; }
        public float M3QuantidadeBombeada { get; set; }
        public ResistenciaTipo ResistenciaTipo { get; set; }
        public float Mpa { get; set; }
        public int Consumo { get; set; }
        public float CustoProjetadoTransporte { get; set; }
        public float CustoServicoReajustado { get; set; }
        public ObraPendenteAprovacaoRequest.UsinaDTO UsinaEntrega { get; set; }

        public int MunicipioCodigo { get; set; }

        public float ObraVolumePorCarga { get; set; }

        public float ObraTempoBtNaObra { get; set; }
        public float obraTempoAteAObra { get; set; }
    }
}
