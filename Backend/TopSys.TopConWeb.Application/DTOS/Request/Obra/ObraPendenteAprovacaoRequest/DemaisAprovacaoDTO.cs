using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest
{
    public class DemaisAprovacaoDTO
    {
        public string Chave { get; set; }

        public int AprovacaoTipoCodigo { get; set; }

        public string UsuarioRequisitante { get; set; }

        public string UsuarioAprovacao { get; set; }

        public DateTime DataHoraSolicitacao { get; set; }
        
        public string Complemento { get; set; }

        public string Observacao { get; set; }

        public AprovacaoTipoDTO AprovacaoTipo { get; set; }
        
        public int StatusAprovacao { get; set; }

        public string LogObservacao { get; set; }
    }
}
