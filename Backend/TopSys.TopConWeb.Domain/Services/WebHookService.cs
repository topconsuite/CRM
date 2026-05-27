using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.WebHook;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class WebHookService : IWebHookService
    {

        private readonly IWebHookRepository _webHookRepository;

        public WebHookService(IWebHookRepository webHookRepository)
        {
            _webHookRepository = webHookRepository;
        }

        public void Adicionar(WebHookDesktop webHook)
        {
            _webHookRepository.Adicionar(webHook);
        }

    }
}
