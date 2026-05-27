using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.LiberacaoAcesso;
using TopSys.TopConWeb.Application.DTOS.Response.LiberacaoAcesso;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ILiberacaoAcessoApplicationService : IApplicationServiceBase<GrupoAcesso>
    {
        void Adicionar(GrupoAcessoInclusaoRequest grupoAcessoRequest, string userRequest);
        void Atualizar(GrupoAcessoAlteracaoRequest grupoAcessoRequest, bool alteraUsuarios, string userRequest);
        void Deletar(int idServico, string userRequest);
        GrupoAcessoResponse ObterGrupoPorCodigo(int grupoAcessoCodigo);
        PagedList<GrupoAcessoResponse> Listar(int pagina, int porPagina, Expression<Func<GrupoAcesso, bool>> filter);
        IEnumerable<LiberacaoAcessoResponse> AdicionarUsuario(IEnumerable<LiberacaoAcessoInclusaoRequest> liberacaoAcessoUsuario, string userRequest);
        void AtualizarPeriodoAusenciaUsuario(IEnumerable<PeriodoAusenciaUsuarioAlteracaoRequest> periodoAusenciaUsuario, string userRequest);
        void AtualizarUsuario(IEnumerable<LiberacaoAcessoAlteracaoRequest> liberacoesAcessosRequest, string userRequest);
        void RemoverUsuario(string usuarioCodigo, string userRequest);
        IEnumerable<LiberacaoAcessoResponse> ListarUsuariosPorGrupoAcesso(int grupoAcessoCodigo);
        IEnumerable<PeriodoAusenciaUsuarioResponse> ListarPeriodosAusenciaPorUsuario(string usuario);
        IEnumerable<LiberacaoAcessoUsuarioResponse> ListarUsuarios();
        bool ObterLiberacaoAcessoUsuario (string usuario);
    }
}
