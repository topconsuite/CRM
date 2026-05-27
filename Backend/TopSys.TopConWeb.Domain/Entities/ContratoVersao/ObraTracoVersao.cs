using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ObraTracoVersao : ObraTracoBase<ObraVersao>
    {
        public int NumeroVersao { get; set; }
    }
}
