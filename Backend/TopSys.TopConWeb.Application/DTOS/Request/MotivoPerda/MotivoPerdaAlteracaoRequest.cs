using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.MotivoPerda
{
    public class MotivoPerdaAlteracaoRequest
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public bool Ativo { get; set; }
    }
}
