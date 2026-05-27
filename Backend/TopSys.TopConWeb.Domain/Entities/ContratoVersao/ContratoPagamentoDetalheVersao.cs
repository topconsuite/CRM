using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ContratoPagamentoDetalheVersao : ContratoPagamentoDetalheBase
    {
        public int NumeroVersao { get; set; }
    }
}
