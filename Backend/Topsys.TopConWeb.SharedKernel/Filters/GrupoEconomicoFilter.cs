using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Filters
{
    public class GrupoEconomicoFilter : IFilter
    {
        public int? Codigo { get; set; }

        public string Descricao { get; set; }

        public int? IntervenienteCodigo { get; set; }
    }
}
