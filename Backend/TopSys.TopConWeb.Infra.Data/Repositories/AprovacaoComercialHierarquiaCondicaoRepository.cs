using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class AprovacaoComercialHierarquiaCondicaoRepository : RepositoryBase<AprovacaoComercialHierarquiaCondicao>, IAprovacaoComercialHierarquiaCondicaoRepository
    {

        public AprovacaoComercialHierarquiaCondicaoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarPorNivelHierarquia(Guid hierarquiaId)
        {
            var result = _context.AprovacaoComercialHierarquiaCondicaos
                .Include(x => x.TipoPessoa)
                .Where(x => x.AprovacaoComercialHierarquiaId == hierarquiaId);

            return result;
        }

        public IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarCondicoesPorNivelHierarquiaTipoPessoa(Guid hierarquiaId, Guid pessoaId)
        {
            var result = _context.AprovacaoComercialHierarquiaCondicaos
                .Include(x => x.TipoPessoa)
                .Where(x => x.AprovacaoComercialHierarquiaId == hierarquiaId && x.TipoPessoaId == pessoaId);

            return result;
        }

        public AprovacaoComercialHierarquiaCondicao ObterCondicaoPorId(Guid id, bool tracking = true)
        {
            var result = _context.AprovacaoComercialHierarquiaCondicaos
                .Include(x => x.TipoPessoa)
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();

            return result;
        }

        public AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorId(Guid id, bool tracking = true)
        {
            var result = _context.AprovacaoComercialHierarquiaCondicaoPagamento
                .Include(x => x.TipoPessoa)
                .Where(x => x.Id == id)
                .Tracking(tracking)
                .FirstOrDefault();

            return result;
        }

        public AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(Guid hierarquiaId, Guid tipoPessoaId, int segmentacaoId, bool tracking = true)
        {

            var result = _context.AprovacaoComercialHierarquiaCondicaoPagamento
                .Include(x => x.TipoPessoa)
                .Where(x => x.AprovacaoComercialHierarquiaId == hierarquiaId
                            && x.TipoPessoaId == tipoPessoaId
                            && x.SegmentacaoId == segmentacaoId)
                .Tracking(tracking)
                .FirstOrDefault();

            return result;

        }

        public AprovacaoComercialHierarquiaCondicao ObterCondicaoPorHierarquiaTipoPessoaTipoValor(Guid hierarquiaId, Guid tipoPessoa, string valor, bool tracking = true)
        {

            var result = _context.AprovacaoComercialHierarquiaCondicaos
                .Include(x => x.TipoPessoa)
                .Where(x => x.AprovacaoComercialHierarquiaId == hierarquiaId && x.TipoPessoaId == tipoPessoa && x.Valor.Equals(valor))
                .Tracking(tracking)
                .FirstOrDefault();

            return result;
        }
        public bool UsuarioTemDireitoTraco(string userId, int obraUsina, int obraNumero, int obraVersao)
        {
            var ativo = _context.AprovacaoComercialUsinas
                .Where(x => x.UsinaId == obraUsina)
                .FirstOrDefault();

            if (ativo == null)
                return false;

            var _obra = new ObraTraco();
            var _obraVersao = new ObraTracoVersao();

            if (obraVersao > 0)
            {
                _obraVersao = _context.ObraTracosVersoes
               .Where(x => x.ObraCodigo == obraNumero)
               .FirstOrDefault();

                if (_obraVersao.UsinaCodigo != obraUsina)
                    return false;

            }
            else
            {
                var obra = _context.ObraTracos
                .Where(x => x.ObraCodigo == obraNumero)
                .FirstOrDefault();

                //if (obra.UsinaCodigo != obraUsina)
                //return false;
            }

            var results = _context.AprovacaoComercialPendenteTracos
                .Where(x => x.ObraNumero == obraNumero && x.ObraVersao == obraVersao && x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                .Tracking(true)
                .ToList();

            if (results == null)
                return false;

            var user = _context.AprovacaoComercialHierarquiaUsuarios
                .Where(x => x.UsuarioId == userId)
                .FirstOrDefault();

            if (user == null)
                return false;

            var hierarquiaUsuario = _context.AprovacaoComercialHierarquias
                .Where(x => x.Id == user.AprovacaoComercialHierarquiaId)
                .FirstOrDefault();

            foreach (var result in results)
            {
                if (result.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao && result.NivelHierarquia == hierarquiaUsuario.NivelAutoridade)
                    return true;
            }

            return false;
        }

        public bool UsuarioTemDireitoBomba(string userId, int obraUsina, int obraNumero, int obraVersao)
        {
            var ativo = _context.AprovacaoComercialUsinas
                .Where(x => x.UsinaId == obraUsina)
                .FirstOrDefault();

            if (ativo == null)
                return false;

            var _obra = new ObraBomba();
            var _obraVersao = new ObraBombaVersao();

            if (obraVersao > 0)
            {
                _obraVersao = _context.ObraBombasVersoes
               .Where(x => x.ObraCodigo == obraNumero)
               .FirstOrDefault();

                if (_obraVersao.UsinaCodigo != obraUsina)
                    return false;

            }
            else
            {
                var obra = _context.ObraBombas
                .Where(x => x.ObraCodigo == obraNumero)
                .FirstOrDefault();

                //if (obra.UsinaCodigo != obraUsina)
                //return false;
            }

            var results = _context.AprovacaoComercialPendenteBombas
                .Where(x => x.ObraNumero == obraNumero && x.ObraVersao == obraVersao && x.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao)
                .Tracking(true)
                .ToList();

            if (results == null)
                return false;

            var user = _context.AprovacaoComercialHierarquiaUsuarios
                .Where(x => x.UsuarioId == userId)
                .FirstOrDefault();

            if (user == null)
                return false;

            var hierarquiaUsuario = _context.AprovacaoComercialHierarquias
                .Where(x => x.Id == user.AprovacaoComercialHierarquiaId)
                .FirstOrDefault();

            foreach (var result in results)
            {
                if (result.AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao && result.NivelHierarquia == hierarquiaUsuario.NivelAutoridade)
                    return true;
            }

            return false;
        }
    }
}
