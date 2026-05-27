using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class EmpresaController : BaseController
    {
        private readonly IEmpresaApplicationService _empresaApplicationService;

        public EmpresaController(IEmpresaApplicationService empresaApplicationService)
        {
            _empresaApplicationService = empresaApplicationService;
        }

        [HttpGet]
        [Route("integrations/company")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EmpresaListar()
        {
            var result = _empresaApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/company/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EmpresaObterPorID([FromUri]int code)
        {
            var result = _empresaApplicationService.ObterPorId(code);

            return CreateResponse(result);
        }
    }
}