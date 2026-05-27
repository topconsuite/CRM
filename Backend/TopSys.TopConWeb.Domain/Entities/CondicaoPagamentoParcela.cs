using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class CondicaoPagamentoParcela
    {
        public int CondicaoPagamentoCodigo { get; set; }
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }

        public int Dias { get; set; }

        public float Percentual { get; set; }
    }
}
