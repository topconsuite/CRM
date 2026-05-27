using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Usuario ObterPorIdSenha(string id, string senha);
        void CadastrarSenha(string id, string novaSenha);
        IDictionary<string, string> ObterDireitosPorId(string id);
        IDictionary<string, string> ObterClaimsVendedores(Usuario usuario);
        float? ObterPercentualDescontoLimitePorId(string id);
        void GravaAcessoAplicacao(AcessoAplicacao acessoAplicacao);
        Dictionary<string, string> ListarUsuariosComGrupos();

    }
}
