using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Interveniente.IntervenienteSimplesResponse
{
    public class IntervenienteSimplesResponse
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }
        public int? GrupoEconomicoCodigo { get; set; }
        public virtual GrupoEconomicoDTO GrupoEconomico { get; set; }
    }
}
