using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Portador
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public int? ContaEmpresaCodigo { get; set; } = 0;
        public int? ContaCodigo { get; set; } = 0;
        public virtual Conta Conta { get; set; }
        
        public string EmiteCobranca { get; set; }
        
        public int Situacao { get; set; }

    }
}
