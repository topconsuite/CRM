using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario Autenticar(string id, string senha);
        Usuario Registrar(string id, string senha, string senhaConfirmacao, string emai);
        IDictionary<string, string> ObterClaimsVendedores(Usuario usuario);
        float? ObterPercentualDescontoLimitePorId(string id);
        Usuario CadastrarSenha(string id, string senha, string senhaConfirmacao);
        void GravarAcessoAplicacao(string usuarioNome, string aplicativo, int programa);
        IEnumerable<Usuario> ObterNomeUsuariosVerificados();

        UsuarioWebFiltro ObterFiltroWebPorId(string usuario, string aplicativo);
        void SalvarFiltroWeb(string usuario, string aplicativo, string json, string filterString);
        Dictionary<string, string> ListarUsuariosComGrupos();

    }
}
