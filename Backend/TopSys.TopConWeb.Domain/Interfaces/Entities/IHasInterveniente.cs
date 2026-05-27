using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IHasInterveniente
    {
        int? IntervenienteCodigo { get; set; }

        Interveniente Interveniente { get; set; }
    }

    public interface IHasIntervenienteRazao
    {
        string IntervenienteRazao { get; set; }
    }
}
