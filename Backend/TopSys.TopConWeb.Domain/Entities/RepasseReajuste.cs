using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class RepasseReajuste
    {
        public DateTime DataInicioValidade { get; set; }

        public int ProdutoCodigo { get; set; }

        public float PercentualCimento { get; set; }
        public float PercentualPedra { get; set; }
        public float PercentualAreia { get; set; }
        public float PercentualMaoDeObra { get; set; }
        public float PercentualDiesel { get; set; }
    }
}
