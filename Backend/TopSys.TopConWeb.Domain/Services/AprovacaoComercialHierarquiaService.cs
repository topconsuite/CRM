using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Services
{
    public class AprovacaoComercialHierarquiaService : ServiceBase<AprovacaoComercialHierarquia>, IAprovacaoComercialHierarquiaService
    {

        private readonly IAprovacaoComercialHierarquiaRepository _aprovacaoComercialHierarquiaRepository;
        private readonly IAprovacaoComercialHierarquiaCondicaoRepository _aprovacaoComercialHierarquiaCondicaoRepository;
        
        public AprovacaoComercialHierarquiaService(
            IAprovacaoComercialHierarquiaRepository aprovacaoComercialHierarquiaRepository,
            IAprovacaoComercialHierarquiaCondicaoRepository aprovacaoComercialHierarquiaCondicaoRepository
            ) : base(aprovacaoComercialHierarquiaRepository)
        {
            _aprovacaoComercialHierarquiaCondicaoRepository = aprovacaoComercialHierarquiaCondicaoRepository;
            _aprovacaoComercialHierarquiaRepository = aprovacaoComercialHierarquiaRepository;
        }

        public void AdicionarCondicao(AprovacaoComercialHierarquiaCondicao aprovacaoComercialHierarquiaCondicao)
        {
            _aprovacaoComercialHierarquiaCondicaoRepository.Adicionar(aprovacaoComercialHierarquiaCondicao);

        }

        public void AdicionarUsuario(AprovacaoComercialHierarquiaUsuario aprovacaoComercialHierarquiaUsuario)
        {
            var hierarquia = _aprovacaoComercialHierarquiaRepository.ObterPorId(aprovacaoComercialHierarquiaUsuario.AprovacaoComercialHierarquiaId, false);

            var usuario = _aprovacaoComercialHierarquiaRepository.ObterUsuarioNivelHierarquiaPorUsuarioUsina(aprovacaoComercialHierarquiaUsuario.UsuarioId, hierarquia.AprovacaoComercialUsinaId);

            if (usuario != null) {
                AssertionConcern.Notify("Usuário", $"Este usuário já esta cadastrado para a hierarquia {usuario.AprovacaoComercialHierarquia.NivelAutoridade} na usina informada.");
                return;
            }

            _aprovacaoComercialHierarquiaRepository.AdicionarUsuario(aprovacaoComercialHierarquiaUsuario);

        }

        public IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarCondicaoPorNivelHierarquia(Guid hierarquiaId)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.ListarPorNivelHierarquia(hierarquiaId);
        }

        public IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarCondicoesPorNivelHierarquiaTipoPessoa(Guid hierarquiaId, Guid pessoaId)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.ListarCondicoesPorNivelHierarquiaTipoPessoa(hierarquiaId, pessoaId);
        }

        public IEnumerable<AprovacaoComercialHierarquia> ListarNivelHierarquiaPorUsina(int usinaId)
        {
            return _aprovacaoComercialHierarquiaRepository.ListarPorUsina(usinaId);
        }

        public IEnumerable<AprovacaoComercialHierarquia> ListarPorAprovacaoComercialUsina(Guid aprovacaoComercialUsinaId)
        {
            return _aprovacaoComercialHierarquiaRepository.ListarPorAprovacaoComercialUsina(aprovacaoComercialUsinaId);
        }

        public IEnumerable<AprovacaoComercialHierarquiaUsuario> ListarUsuariosPorNivelHierarquia(Guid hierarquiaId)
        {
            return _aprovacaoComercialHierarquiaRepository.ListarUsuariosPorNivelHierarquia(hierarquiaId);
        }

        public AprovacaoComercialHierarquia ObterPorId(Guid id)
        {
            return _aprovacaoComercialHierarquiaRepository.ObterPorId(id, true);
        }

        // ------------------------- Usuário -----------------------------------------------------------------------------------------

        public AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuario(string usuarioId)
        {
            return _aprovacaoComercialHierarquiaRepository.ObterUsuarioNivelHierarquiaPorUsuario(usuarioId);
        }

        public void RemoverUsuario(Guid aprovComUsuarioId)
        {

            var usuario = _aprovacaoComercialHierarquiaRepository.ObterUsuarioNivelHierarquiaPorId(aprovComUsuarioId);

            if (usuario == null)
                AssertionConcern.Notify("Usuário", "Usuário informado não encontrado.");

            _aprovacaoComercialHierarquiaRepository.RemoverUsuario(usuario);

        }

        public AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuarioUsina(string usuarioId, Guid aprovacaoComercialId)
        {
            return _aprovacaoComercialHierarquiaRepository.ObterUsuarioNivelHierarquiaPorUsuarioUsina(usuarioId, aprovacaoComercialId);
        }

        // ------------------------- Tipo Pessoa -----------------------------------------------------------------------------------------

        public IEnumerable<AprovacaoComercialTipoPessoa> ListarTipoPessoa()
        {
            return _aprovacaoComercialHierarquiaRepository.ListarTipoPessoa();
        }

        public AprovacaoComercialTipoPessoa ObterTipoPessoaPorSigla(string sigla)
        {
            return _aprovacaoComercialHierarquiaRepository.ObterTipoPessoaPorSigla(sigla);
        }

        // ------------------------- Condição -----------------------------------------------------------------------------------------

        public AprovacaoComercialHierarquiaCondicao ObterCondicaoPorId(Guid id)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.ObterCondicaoPorId(id);
        }

        public AprovacaoComercialHierarquiaCondicao ObterCondicaoPorHierarquiaTipoPessoaTipoValor(Guid hierarquiaId, Guid tipoPessoa, string valor)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.ObterCondicaoPorHierarquiaTipoPessoaTipoValor(hierarquiaId, tipoPessoa, valor);
        }

        public bool UsuarioTemDireitoTraco(string userId, int obraUsina, int obraNumero, int obraVersao)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.UsuarioTemDireitoTraco(userId, obraUsina, obraNumero, obraVersao);
        }

        public bool UsuarioTemDireitoBomba(string userId, int obraUsina, int obraNumero, int obraVersao)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.UsuarioTemDireitoBomba(userId, obraUsina, obraNumero, obraVersao);
        }
        public IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentes(int obraUsina, int obraNumero, int obraVersao)
        {
            return _aprovacaoComercialHierarquiaRepository.ListarAprovacoesPendentes(obraUsina, obraNumero, obraVersao);
        }

        public AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(Guid hierarquiaId, Guid tipoPessoaId, int segmentacaoId, bool tracking = true)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(hierarquiaId, tipoPessoaId, segmentacaoId, tracking);
        }

        public AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorId(Guid id, bool tracking = true)
        {
            return _aprovacaoComercialHierarquiaCondicaoRepository.ObterCondicaoPagamentoPorId(id, tracking);
        }

    }
}
