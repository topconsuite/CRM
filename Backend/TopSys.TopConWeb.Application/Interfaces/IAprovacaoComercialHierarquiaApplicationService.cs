using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Response.AprovacaoComercial;
using TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Response.Usuario;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IAprovacaoComercialHierarquiaApplicationService
    {

        IEnumerable<AprovacaoComercialHierarquia> ListarNivelHierarquiaPorUsina(Guid aprovacaoComercialUsinaId);
        int ObterProximoNivelHierarquiaPorAprovacaoComercialUsina(Guid aprovacaoComercialUsinaId);

        AprovacaoComercialHierarquiaResponse AdicionarHierarquia(AprovacaoComercialHierarquiaInsercaoRequest aprovacaoComercialHierarquiaInsercaoRequest);
        void AtualizarHierarquia(AprovacaoComercialHierarquiaAlteracaoRequest aprovacaoComercialHierarquiaAlteracaoRequest);

        AprovacaoComercialHierarquiaUsuarioResponse AdicionarUsuario(AprovacaoComercialHierarquiaUsuarioInsercaoRequest aprovacaoComercialHierarquiaUsuario);
        void RemoverUsuario(Guid aprovComUsuarioId);
        IEnumerable<AprovacaoComercialHierarquiaUsuarioResponse> ListarUsuariosPorNivelHierarquia(Guid hierarquiaId);
        AprovacaoComercialHierarquiaUsuarioResponse ObterUsuarioNivelHierarquiaPorUsuario(string usuarioId);

        void AdicionarCondicao(AprovacaoComercialHierarquiaCondicaoInsercaoRequest condicaoRequest, bool commit = false);
        void AtualizarCondicao(AprovacaoComercialHierarquiaCondicaoInsercaoRequest condicaoRequest, bool commit = false);
        IEnumerable<AprovacaoComercialHierarquiaCondicaoResponse> ListarCondicaoPorNivelHierarquia(Guid hierarquiaId);
        IEnumerable<AprovacaoComercialHierarquiaCondicaoResponse> ListarCondicoesPorNivelHierarquiaTipoPessoa(Guid hierarquiaId, Guid pessoaId);

        IEnumerable<AprovacaoComercialTipoPessoaResponse> ListarTipoPessoa();
        IEnumerable<UsuarioAprovacaoComercialResponse> ListarUsuarios();

        void AdicionarCondicoes(AprovacaoComercialHierarquiaCondicaoInsercaoRequest[] condicoes);
        AprovacaoComercialHierarquiaCondicaoResponse ObterCondicaoPorId(Guid id);
        AprovacaoComercialHierarquiaCondicaoResponse ObterCondicaoPorHierarquiaTipoPessoaTipoValor(Guid hierarquiaId, Guid tipoPessoa, string Valor);

        AprovacaoComercialHierarquiaUsuarioDireitoResponse UsuarioTemDireitoAprovacaoComercialObra(string userId, int obraUsina, int obraNumero, int obraVersao);
        AprovacaoComercialDadosResponse ListarDadosAprovacoesComercialObra(int obraUsina, int obraNumero, int obraVersao);


        AprovacaoComercialCondicaoPagamentoResponse ObterCondicaPagamentoPorHierarquiaTipoPessoaSegmentacao(Guid hierarquiaId, Guid tipoPessoaId, int segmentacaoId);

        string AtualizarCondicaoPagamento(AprovacaoComercialCondicaoPagamentoAtualizarRequest[] request);

    }
}
