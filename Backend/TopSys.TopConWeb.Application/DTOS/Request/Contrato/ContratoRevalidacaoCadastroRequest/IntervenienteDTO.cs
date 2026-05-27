using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest
{
    public class IntervenienteDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }

        public float LimiteValor { get; set; }

        public DateTime? LimiteData { get; set; }

        public int BloqueioMotivoCodigo { get; set; }

        public CadastroGeralDTO BloqueioMotivo { get; set; }

    }
}
