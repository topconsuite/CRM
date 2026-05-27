using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.WebHook;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IWebHookRepository
    {

        void Adicionar(WebHookDesktop webHook);

    }
}
