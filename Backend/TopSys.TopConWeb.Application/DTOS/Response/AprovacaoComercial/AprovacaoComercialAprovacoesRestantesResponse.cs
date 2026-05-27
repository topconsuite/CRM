using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialAprovacoesRestantesResponse
    {
        public string Hierarquia { get; set; }
        public int QtdAprovacoes { get; set; }
        public int AprovacoesNecessarias { get; set; }
        public bool AprovacaoObrigatoria { get; set; }
    }
}
