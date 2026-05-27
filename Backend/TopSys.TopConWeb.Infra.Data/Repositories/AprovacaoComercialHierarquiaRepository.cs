using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class AprovacaoComercialHierarquiaRepository : RepositoryBase<AprovacaoComercialHierarquia>, IAprovacaoComercialHierarquiaRepository
    {

        public AprovacaoComercialHierarquiaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public void AdicionarUsuario(AprovacaoComercialHierarquiaUsuario aprovacaoComercialHierarquiaUsuario)
        {
            _context.AprovacaoComercialHierarquiaUsuarios.Add(aprovacaoComercialHierarquiaUsuario);
        }

        public void RemoverUsuario(AprovacaoComercialHierarquiaUsuario aprovacaoComercialHierarquiaUsuario)
        {
            _context.AprovacaoComercialHierarquiaUsuarios.Remove(aprovacaoComercialHierarquiaUsuario);
        }

        public AprovacaoComercialHierarquia ObterPorId(Guid id, bool tracking = false)
        {
            var result = _context.AprovacaoComercialHierarquias
                .Include(x => x.AprovacaoComercialUsina)
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();

            return result;
        }

        public IEnumerable<AprovacaoComercialHierarquia> ListarPorUsina(int usinaId)
        {
            var result = _context.AprovacaoComercialHierarquias
                .Include(x => x.AprovacaoComercialUsina)
                .Include(x => x.Condicoes)
                .AsNoTracking()
                .Where(x => x.AprovacaoComercialUsina.UsinaId == usinaId);

            return result;
        }

        public IEnumerable<AprovacaoComercialHierarquia> ListarPorAprovacaoComercialUsina(Guid aprovacaoComercialUsinaId)
        {
            var result = _context.AprovacaoComercialHierarquias
                .Include(x => x.AprovacaoComercialUsina)
                .Include(x => x.Condicoes)
                .Where(x => x.AprovacaoComercialUsinaId == aprovacaoComercialUsinaId)
                .OrderByDescending(x => x.NivelAutoridade);

            return result;
        }

        public IEnumerable<AprovacaoComercialTipoPessoa> ListarTipoPessoa()
        {
            return _context.AprovacaoComercialTipoPessoas.ToList();
        }

        public AprovacaoComercialTipoPessoa ObterTipoPessoaPorSigla(string sigla)
        {
            return _context.AprovacaoComercialTipoPessoas.Where(x => x.Sigla.Equals(sigla)).FirstOrDefault();
        }

        public IEnumerable<AprovacaoComercialHierarquiaUsuario> ListarUsuariosPorNivelHierarquia(Guid hierarquiaId)
        {
            var result = _context.AprovacaoComercialHierarquiaUsuarios
                .Where(x => x.AprovacaoComercialHierarquiaId == hierarquiaId);

            return result;
        }

        public AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuario(string usuarioId)
        {
            var result = _context.AprovacaoComercialHierarquiaUsuarios
                .Where(x => x.UsuarioId.Equals(usuarioId)).Tracking(true).FirstOrDefault();

            return result;
        }

        public AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuarioUsina(string usuarioId, Guid aprovacaoComercialId)
        {
            var result = _context.AprovacaoComercialHierarquiaUsuarios
                .Include(x => x.AprovacaoComercialHierarquia)
                .Where(x => x.UsuarioId.Equals(usuarioId) && x.AprovacaoComercialHierarquia.AprovacaoComercialUsinaId == aprovacaoComercialId)
                .FirstOrDefault();

            return result;
        }

        public AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorId(Guid id)
        {
            var result = _context.AprovacaoComercialHierarquiaUsuarios
                .Where(x => x.Id == id).Tracking(true).FirstOrDefault();

            return result;
        }
        public IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentes(int obraUsina, int obraNumero, int obraVersao = 0)
        {
            var result = _context.AprovacaoComercialPendentes
                .Where(x => x.ObraUsina == obraUsina && x.ObraNumero == obraNumero && x.ObraVersao == obraVersao)
                .ToList();

            return result;
        }

    }
}
