using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class AcessoAplicacao
    {
        public string Aplicativo { get; set; }
        public int Programa { get; set; }
        public string UsuarioId { get; set; }
        
        public string UsuarioEmail { get; set; }
        //public Usuario Usuario { get; set; }
        public int ClienteCodigo { get; set; }
        public string ClienteNome { get; set; }

        public AcessoAplicacao()
        { }

        public AcessoAplicacao(string aplicativo, int programa, string usuarioId, string usuarioEmail, int clienteCodigo, string clienteNome)
        {
            Aplicativo = aplicativo;
            Programa = programa;
            UsuarioId = usuarioId;
            UsuarioEmail = usuarioEmail;
            ClienteCodigo = clienteCodigo;
            ClienteNome = clienteNome;
        }
    }
}
