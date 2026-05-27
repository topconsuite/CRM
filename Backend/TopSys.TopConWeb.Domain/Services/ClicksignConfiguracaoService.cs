using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ClicksignConfiguracaoService : ServiceBase<ClicksignConfiguracao>, IClicksignConfiguracaoService
    {
        private readonly IClicksignConfiguracaoRepository _clicksignConfiguracaoRepository;

        public ClicksignConfiguracaoService(IClicksignConfiguracaoRepository clicksignConfiguracaoRepository)
            : base(clicksignConfiguracaoRepository)
        {
            _clicksignConfiguracaoRepository = clicksignConfiguracaoRepository;
        }

        public ClicksignConfiguracao ObterConfiguracaoPorUsina(int usinaId)
        {
            return _clicksignConfiguracaoRepository.ObterConfiguracaoPorUsina(usinaId);
        }
    }
}
