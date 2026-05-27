using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IClicksignConfiguracaoRepository : IRepositoryBase<ClicksignConfiguracao>
    {
        ClicksignConfiguracao ObterConfiguracaoPorUsina(int usinaId);

        IEnumerable<Usina> ListarUsinasPorConfiguracao(int configuracaoId);

        void VincularUsina(int configuracaoId, int usinaId);

        void DesvincularUsina(int usinaId);
    }
}
