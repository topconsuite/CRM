using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class LiberacaoAcessoService : ServiceBase<GrupoAcesso>, ILiberacaoAcessoService
    {
        private ILiberacaoAcessoRepository _liberacaoAcessoRepository;

        public LiberacaoAcessoService(ILiberacaoAcessoRepository liberacaoAcessoRepository) : base(liberacaoAcessoRepository)
        {
            _liberacaoAcessoRepository = liberacaoAcessoRepository;
        }

        public PagedList<GrupoAcesso> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<GrupoAcesso, bool>> filter)
        {
            var pagedList = _liberacaoAcessoRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }

        public LiberacaoAcesso ObterLiberacaoAcessoPorUsuario(string usuario)
        {
            return _liberacaoAcessoRepository.ObterLiberacaoAcessoPorUsuario(usuario);
        }

        // ------------------------- Usuário -----------------------------------------------------------------------------------------

        public IEnumerable<LiberacaoAcesso> ListarUsuariosPorGrupoAcesso(int grupoAcessoCodigo)
        {
            return _liberacaoAcessoRepository.ListarUsuariosPorGrupoAcesso(grupoAcessoCodigo);
        }

        public IEnumerable<PeriodoAusenciaUsuario> ListarPeriodosAusenciaPorUsuario(string usuario)
        {
            return _liberacaoAcessoRepository.ListarPeriodosAusenciaPorUsuario(usuario);
        }

        public void AtualizarLiberacaoAcessoUsuario(LiberacaoAcesso liberacao)
        {
            _liberacaoAcessoRepository.AtualizarLiberacaoAcessoUsuario(liberacao);
        }
    }
}