using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class SsoController : BaseController
    {
        private readonly ISsoApplicationService _ssoApplicationService;

        public SsoController(ISsoApplicationService ssoApplicationService)
        {
            _ssoApplicationService = ssoApplicationService;
        }

        [HttpGet]
        [Route("v1/sso/parametros/azure-ad")]
        public Task<HttpResponseMessage> ObterParametroSSOAzureAd()
        {
            var parametroAtivo = _ssoApplicationService.ObterParametroAtivoAzureAd();

            return CreateResponse(HttpStatusCode.OK, parametroAtivo);
        }
    }
}