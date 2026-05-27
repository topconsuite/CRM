using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.WebHook;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IWebHookService
    {

        void Adicionar(WebHookDesktop webHook);

    }
}
