using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Proposta.PropostaSimplesResponse
{
    public class IntervenienteDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }

        public virtual CadastroGeralDTO BloqueioMotivo { get; set; }
        public string BloqueioObservacao { get; set; }
    }
}
