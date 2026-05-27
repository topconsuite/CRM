using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraParaAnaliseCadastroResponse
{
    public class IntervenienteDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }
        public float LimiteValor { get; set; }
        public DateTime? LimiteData { get; set; }
        public int BloqueioMotivoCodigo { get; set; }
        public string Observacao { get; set; }
        public string BloqueioObservacao { get; set; }
        public CadastroGeralDTO BloqueioMotivo { get; set; }
        public string RetemIss { get; set; }
        public string IdAprovacaoRetencaoIss { get; set; }
        public string IntervenienteTipo { get; set; }
        public int GrupoEconomicoCodigo { get; set; }
        public GrupoEconomicoDTO GrupoEconomico { get; set; }
    }
}
