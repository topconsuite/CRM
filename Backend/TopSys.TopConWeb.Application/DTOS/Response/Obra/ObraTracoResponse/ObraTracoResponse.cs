using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracoResponse
{
    public class ObraTracoResponse
    {
        public Request.Obra.ObraPendenteAprovacaoRequest.UsinaDTO Usina { get; set; }

        public int ObraCodigo { get; set; }

        public int PropostaAno { get; set; }

        public int PropostaNumero { get; set; }

        public int Sequencia { get; set; }

        public Request.Obra.ObraPendenteAprovacaoRequest.UsoDTO Uso { get; set; }

        public Request.Obra.ObraPendenteAprovacaoRequest.PedraDTO Pedra { get; set; }

        public Request.Obra.ObraPendenteAprovacaoRequest.SlumpDTO Slump { get; set; }

        public Request.Obra.ObraPendenteAprovacaoRequest.SlumpDTO SlumpNominal { get; set; }

        public ResistenciaTipo ResistenciaTipo { get; set; }

        public float Mpa { get; set; }

        public int Consumo { get; set; }

        public float M3Quantidade { get; set; }

        public float M3PrecoTabela { get; set; }

        public float M3PrecoProposto { get; set; }

        public float M3PrecoAjustado { get; set; }

        public float DescontoPercentual { get; set; }

        public string PecaConcretar { get; set; }

        public float ValorServico { get; set; }

        public float PrecoConcorrencia { get; set; }

        public string DescontoSolicitante { get; set; }

        public string AprovacaoTipo { get; set; }

        public string AprovacaoVerbal { get; set; }

        public string AprovacaoObservacao { get; set; }

        public string AprovacaoOperacao { get; set; }

        public string AprovacaoCiente { get; set; }

        public int StatusAprovacao { get; set; }

        public string Justificativa { get; set; }

        public float PrecoReajustadoAnterior { get; set; }

        public DateTime? DataUltimoReajuste { get; set; }

        public float PrecoReajustadoAtual { get; set; }

        public string Observacao { get; set; }

        public string DescricaoPersonalizada { get; set; }

        public float M3QuantidadeBombeada { get; set; }

        public float MargemPosTransporte { get; set; }
        public float Ebitda { get; set; }

        public float IssDedutivel { get; set; }

        public float ImpostoAplicadoEstadual { get; set; }

        public float ImpostoAplicadoFederal { get; set; }
        public float CustoBombagem { get; set; }

        public float CustoServicoReajustado { get; set; }

        public float CustoProjetadoTransporte { get; set; }

        public float TotalImpostos { get; set; }

        public float CustoServicoAnterior { get; set; }
        public string Ativo { get; set; }

        public Boolean Inativo
        {
            get
            {
                return Ativo == "N";
            }
            set
            {
                if (value)
                    Ativo = "N";
                else
                    Ativo = "S";
            }
        }
    } 
}
