using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial
{
    public class AprovacaoComercialHierarquiaUsuarioDireitoResponse
    {

        public bool UtilizaAprovacaoComercialAlcada { get; set; }
        public bool ObraEstaReprovado { get; set; }

        public List<int> DireitoTracos { get; set; }
        public List<int> DireitoBombas { get; set; }
        public bool DireitoVolume { get; set; }
        public bool DireitoCondicaoPagamento { get; set; }


    }

}
