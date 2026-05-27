using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Usuario;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IUsuarioApplicationService : IApplicationServiceBase<Usuario>
    {
        AutenticarUsuarioResponse Autenticar(string login, string senha);
        CadastrarSenhaUsuarioResponse CadastrarSenhaUsuario(CadastrarSenhaUsuarioRequest usuario);
        RegistrarUsuarioResponse Registrar(RegistrarUsuarioRequest registrarUsuarioRequest);
        IDictionary<string, string> ObterClaimsVendedores(AutenticarUsuarioResponse usuario);
        float? ObterPercentualDescontoLimitePorId(string id);
        void GravarAcessoAplicacao(string usuario, string aplicativo, int programa);
        UsuarioWebFiltroResponse ObterFiltroWebPorId(string usuario, string aplicativo);
        void SalvarFiltroWeb(string usuario, UsuarioWebFiltroSalvarRequest request);
        Usuario ObterUsuarioPeloEmail(string email);
    }
}
