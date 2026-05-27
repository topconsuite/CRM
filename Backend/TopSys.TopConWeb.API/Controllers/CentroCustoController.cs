using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.CentroCusto;
using TopSys.TopConWeb.API.Security;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CentroCustoController : BaseController
    {
        private readonly ICentroCustoApplicationService _centroCustoApplicationService;

        public CentroCustoController(ICentroCustoApplicationService centroCustoApplicationService)
        {
            _centroCustoApplicationService = centroCustoApplicationService;
        }

        [HttpPost]
        [Route("integrations/financial-cost-center")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CentroCustoAdicionar([FromBody] CentroCustoRequest request)
        {
            var result = _centroCustoApplicationService.Adicionar(request.CentrosCusto);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/financial-cost-center/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CentroCustoAtualizarPorId([FromUri] int code, [FromBody] CentroCustoAtualizarRequest request)
        {
            var result = _centroCustoApplicationService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/financial-cost-center")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CentroCustoListar()
        {
            var result = _centroCustoApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/financial-cost-center/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CentroCustoObterPorID([FromUri]int code)
        {
            var result = _centroCustoApplicationService.ObterPorId(code);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/financial-cost-center/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CentroCustoDeletarPorId([FromUri] int code)
        {
            var result = _centroCustoApplicationService.DeletarPorId(code);

            return CreateResponse(result);
        }
    }
}