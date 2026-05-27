using System.Linq;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class SsoApplicationService : ApplicationServiceBase<ParametrosSSO>, ISsoApplicationService
    {
        private readonly ISsoService _service;
        public SsoApplicationService(ISsoService ssoService,
            IUnitOfWork unityOfWork) : base(ssoService, unityOfWork)
        {
            _service = ssoService;
        }

        public ParametrosSSO ObterParametroAtivoAzureAd()
        {
            var parametros = _service.ListarFiltrados(t => t.TipoProvedor == ParametrosSSO.ETipoProvedor.Microsoft && t.Habilitado);

            return parametros.FirstOrDefault();
        }

        public ParametrosSSO ObterParametroAtivoB2C()
        {
            var parametros = _service.ListarFiltrados(t => t.TipoProvedor == ParametrosSSO.ETipoProvedor.B2C && t.Habilitado);

            return parametros.FirstOrDefault();
        }
    }
}
