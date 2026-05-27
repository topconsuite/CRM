using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial
{
    public interface IAprovacaoComercialHierarquiaCondicaoRepository : IRepositoryBase<AprovacaoComercialHierarquiaCondicao>
    {

        IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarPorNivelHierarquia(Guid hierarquiaId);

        IEnumerable<AprovacaoComercialHierarquiaCondicao> ListarCondicoesPorNivelHierarquiaTipoPessoa(Guid hierarquiaId, Guid pessoaId);

        AprovacaoComercialHierarquiaCondicao ObterCondicaoPorId(Guid id, bool tracking = true);
        AprovacaoComercialHierarquiaCondicao ObterCondicaoPorHierarquiaTipoPessoaTipoValor(Guid hierarquiaId, Guid tipoPessoa, string valor, bool tracking = true);
        bool UsuarioTemDireitoTraco(string userId, int obraUsina, int obraNumero, int obraVersao);
        bool UsuarioTemDireitoBomba(string userId, int obraUsina, int obraNumero, int obraVersao);

        AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorHierarquiaTipoPessoaSegmentacao(Guid hierarquiaId, Guid tipoPessoaId, int condicaoPagamentoId, bool tracking = true);
        AprovacaoComercialHierarquiaCondicaoPagamento ObterCondicaoPagamentoPorId(Guid id, bool tracking = true);

    }
}
