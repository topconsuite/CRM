using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial
{
    public interface IAprovacaoComercialHierarquiaRepository : IRepositoryBase<AprovacaoComercialHierarquia>
    {

        IEnumerable<AprovacaoComercialTipoPessoa> ListarTipoPessoa();
        IEnumerable<AprovacaoComercialHierarquia> ListarPorUsina(int usinaId);
        AprovacaoComercialHierarquia ObterPorId(Guid id, bool tracking = false);

        void AdicionarUsuario(AprovacaoComercialHierarquiaUsuario aprovacaoComercialHierarquiaUsuario);
        void RemoverUsuario(AprovacaoComercialHierarquiaUsuario aprovacaoComercialHierarquiaUsuario);
        IEnumerable<AprovacaoComercialHierarquiaUsuario> ListarUsuariosPorNivelHierarquia(Guid hierarquiaId);
        AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuario(string usuarioId);
        AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorUsuarioUsina(string usuarioId, Guid aprovacaoComercialId);
        IEnumerable<AprovacaoComercialHierarquia> ListarPorAprovacaoComercialUsina(Guid aprovacaoComercialUsinaId);
        AprovacaoComercialTipoPessoa ObterTipoPessoaPorSigla(string sigla);
        AprovacaoComercialHierarquiaUsuario ObterUsuarioNivelHierarquiaPorId(Guid id);
        IEnumerable<AprovacaoComercialPendente> ListarAprovacoesPendentes(int obraUsina, int obraNumero, int obraVersao);

    }
}
