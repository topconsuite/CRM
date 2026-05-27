using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ILiberacaoAcessoRepository : IRepositoryBase<GrupoAcesso>
    {
        PagedList<GrupoAcesso> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<GrupoAcesso, bool>> filter);
        LiberacaoAcesso ObterLiberacaoAcessoPorUsuario(string usuario);
        IEnumerable<LiberacaoAcesso> ListarUsuariosPorGrupoAcesso(int grupoAcessoCodigo);
        IEnumerable<PeriodoAusenciaUsuario> ListarPeriodosAusenciaPorUsuario(string usuario);
        void AtualizarLiberacaoAcessoUsuario(LiberacaoAcesso liberacao);
    }
}
