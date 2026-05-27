using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaInsercaoRequest
    {

        public Guid AprovacaoComercialUsinaId { get; set; }

        public string Titulo { get; set; }
        public int NivelAutoridade { get; set; }
        public int QuantidadeAprovacoesNecessarias { get; set; }

        public bool AprovacaoObrigatoria { get; set; }

    }
}
