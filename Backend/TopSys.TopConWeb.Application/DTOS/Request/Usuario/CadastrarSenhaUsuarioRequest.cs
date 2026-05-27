using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Request.Usuario
{
    public class CadastrarSenhaUsuarioRequest
    {
        public string IdUsuario { get; set; }

        public string Senha { get; set; }

        public string SenhaConfirmacao { get; set; }

    }
}
