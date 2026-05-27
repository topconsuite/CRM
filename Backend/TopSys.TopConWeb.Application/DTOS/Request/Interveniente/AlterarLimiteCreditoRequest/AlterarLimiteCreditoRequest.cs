using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Interveniente.AlterarLimiteCreditoRequest
{
    public class AlterarLimiteCreditoRequest
    {
        public int Codigo { get; set; }
        public float LimiteValor { get; set; }
        public DateTime? LimiteData { get; set; }
        public string BloqueioObservacao { get; set; }
        public CadastroGeralDTO BloqueioMotivo { get; set; }
    }
}
