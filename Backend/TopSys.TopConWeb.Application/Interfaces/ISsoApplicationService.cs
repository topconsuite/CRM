using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ISsoApplicationService
    {
        ParametrosSSO ObterParametroAtivoAzureAd();
    }
}
