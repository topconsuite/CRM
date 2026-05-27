using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest
{
    public class ObraTaxaDTO
    {
        public int UsinaCodigo { get; set; }

        public int ObraCodigo { get; set; }

        public int Sequencia { get; set; }

        public string Descricao { get; set; }

        public bool IsPersonalizada { get; set; }

        public string Selecionada { get; set; }

        public string AprovacaoSolicitante { get; set; }
        
        public int StatusAprovacao { get; set; }

        public string LogObservacao { get; set; }

        public bool Nova { get; set; }

        public string Antecedencia { get; set; }

        public int Quantidade { get; set; }
    }
}
