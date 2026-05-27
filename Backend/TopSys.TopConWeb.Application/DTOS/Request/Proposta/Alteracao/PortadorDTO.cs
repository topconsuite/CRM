using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao
{
    public class PortadorDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }
        public virtual ContaDTO Conta { get; set; }
    }
}
