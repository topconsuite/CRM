using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IClicksignConfiguracaoService : IServiceBase<ClicksignConfiguracao>
    {
        ClicksignConfiguracao ObterConfiguracaoPorUsina(int usinaId);
    }
}
