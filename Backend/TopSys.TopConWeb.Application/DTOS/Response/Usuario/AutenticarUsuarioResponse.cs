using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Usuario
{
    public class AutenticarUsuarioResponse
    {
        public string UsuarioId { get; set; }

        public string Nome { get; set; }
        
        public IDictionary<string,string> Direitos { get; set; }
    }
}
