using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoRevalidacaoCadastroResponse
{
    public class ContratoRevalidacaoCadastroResponse
    {

        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public IntervenienteDTO Interveniente { get; set; }

    }
}
