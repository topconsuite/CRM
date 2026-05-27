using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraPendenteAprovacaoResponse;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracosResponse
{
    public class ObraTracoDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }
        public float Mpa { get; set; }
        public float Fck { get; set; }
        public int Consumo { get; set; }

        public string Resistencia { get; set; }
        public ResistenciaTipo ResistenciaTipo { get; set; }
        public UsoDTO Uso { get; set; }

        public PedraDTO Pedra { get; set; }

        public SlumpDTO Slump { get; set; }

        public float M3Quantidade { get; set; }

        public float M3PrecoTabela { get; set; }

        public float M3PrecoProposto { get; set; }

        public float M3PrecoAjustado { get; set; }

        public float DescontoPercentual { get; set; }

        public float ValorServico { get; set; }

        public string DescontoSolicitante { get; set; }

        public string AprovacaoVerbal { get; set; }

        public string AprovacaoObservacao { get; set; }

        public int StatusAprovacao { get; set; }

        public string Justificativa { get; set; }

        public DateTime? DataUltimoReajuste { get; set; }

        public float PrecoReajustadoAtual { get; set; }

        public float M3QuantidadeBombeada { get; set; }

        public float CustoServicoReajustado { get; set; }
        public string Ativo { get; set; }

        public int? NumeracaoProduto { get; set; }

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
