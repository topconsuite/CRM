using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class FilialController : BaseController
    {

        private readonly IFilialApplicationService _filialApplicationService;

        public FilialController(IFilialApplicationService filialApplicationService)
        {
            _filialApplicationService = filialApplicationService;
        }

        [HttpGet]
        [Route("v1/filial/{idFilial}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorId(int idFilial)
        {
            var filial = _filialApplicationService.ObterPorId(idFilial);

            return CreateResponse(HttpStatusCode.OK, filial);
        }

        [HttpGet]
        [Route("v1/filiais")]
        [Authorize]
        public Task<HttpResponseMessage> Listar()
        {
            var filiais = _filialApplicationService.Listar();

            return CreateResponse(HttpStatusCode.OK, filiais);
        }

        [HttpGet]
        [Route("integrations/fiscal-branch")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FilialFiscalListar()
        {
            var result = _filialApplicationService.FilialFiscalListar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/fiscal-branch/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FilialFiscalObterPorId([FromUri] int code)
        {
            var result = _filialApplicationService.FilialFiscalObterPorId(code);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/fiscal-branch/by-cost-center/{cost_center}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FilialFiscalObterPorCentroCusto([FromUri(Name = "cost_center")] int costCenter)
        {
            var result = _filialApplicationService.FilialFiscalObterPorCentroCusto(costCenter);

            return CreateResponse(result);
        }
    }
}