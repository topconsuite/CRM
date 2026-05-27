using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IAprovacaoComercialHierarquiaService : IServiceBase<AprovacaoComercialHierarquia>
    {

        IEnumerable<AprovacaoComercialHierarquia> ListarNivelHierarquiaPorUsina(int usinaId);
        AprovacaoComercialHierarquia ObterPorId(Guid id);

        void AdicionarUsuario(AprovacaoComercialHierarquiaUsuario aprovacaoComercialHierarquiaUsuario);
        void RemoverUsuario(Guid aprovComUsuarioId);
        IEnumerable<AprovacaoComercialHierarquiaUsuario> ListarUsuariosPorNivelHierarquia(Guid hierarquiaId);
        AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuario(string usuarioId);
        AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuarioUsina(string usuarioId, Guid aprovacaoComercialId);

        void AdicionarCondicao(AprovacaoComercialHierarquiaCondicao aprovacaoComercialHierarquiaCondicao);
        IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarCondicaoPorNivelHierarquia(Guid hierarquiaId);

        AprovacaoComercialHierarquiaCondicao ObterCondicaoPorId(Guid id);
        AprovacaoComercialHierarquiaCondicao ObterCondicaoPorHierarquiaTipoPessoaTipoValor(Guid hierarquiaId, Guid tipoPessoa, string valor);

        IEnumerable<AprovacaoComercialHierarquia> ListarPorAprovacaoComercialUsina(Guid aprovacaoComercialUsinaId);
        IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarCondicoesPorNivelHierarquiaTipoPessoa(Guid hierarquiaId, Guid pessoaId);

        IEnumerable<AprovacaoComercialTipoPessoa> ListarTipoPessoa();
        AprovacaoComercialTipoPessoa ObterTipoPessoaPorSigla(string sigla);
        bool UsuarioTemDireitoTraco(string userId, int obraUsina, int obraNumero, int obraVersao);
        bool UsuarioTemDireitoBomba(string userId, int obraUsina, int obraNumero, int obraVersao);
        IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentes(int obraUsina, int obraNumero, int obraVersao);

        AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(Guid hierarquiaId, Guid tipoPessoaId, int segmentacaoId, bool tracking = true);
        AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorId(Guid id, bool tracking = true);

    }
}
