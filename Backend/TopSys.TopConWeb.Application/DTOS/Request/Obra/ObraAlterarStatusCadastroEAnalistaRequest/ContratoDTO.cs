using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest
{
    public class ContratoDTO
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }
        public FuncionarioDTO Analista { get; set; }
        public bool FaturamentoPendente { get; set; }
        public int ModeloDocumentoRemessaConcreto { get; set; }
        public int ModeloDocumentoRemessaBomba { get; set; }
        public int ModeloItensDanfeERomaneio { get; set; }
        public float PercentualRetencaoContratual { get; set; }
        public string MaoObraPropria { get; set; }
        public float PercentualLocacao { get; set; }
    }
}
