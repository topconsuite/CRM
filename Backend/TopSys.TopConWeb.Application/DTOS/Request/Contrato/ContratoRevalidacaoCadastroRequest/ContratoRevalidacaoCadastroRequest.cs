using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest
{
    public class ContratoRevalidacaoCadastroRequest
    {

        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public string observacaoLog { get; set; }

        public IntervenienteDTO Interveniente { get; set; }

    }
}
