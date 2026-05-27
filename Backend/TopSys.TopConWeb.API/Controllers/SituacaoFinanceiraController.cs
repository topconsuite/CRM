using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.SituacaoFinanceira;
using TopSys.TopConWeb.API.Security;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class SituacaoFinanceiraController : BaseController
    {
        private readonly ISituacaoFinanceiraApplicationService _situacaoFinanceiraApplicationService;

        public SituacaoFinanceiraController(ISituacaoFinanceiraApplicationService situacaoFinanceiraApplicationService)
        {
            _situacaoFinanceiraApplicationService = situacaoFinanceiraApplicationService;
        }

        [HttpPost]
        [Route("integrations/finance-situation")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> SituacaoFinanceiraAdicionar([FromBody] SituacaoFinanceiraRequest request)
        {
            var result = _situacaoFinanceiraApplicationService.Adicionar(request.SituacoesFinanceiras);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/finance-situation/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> SituacaoFinanceiraAtualizarPorId([FromUri] int code, [FromBody] SituacaoFinanceiraAtualizarRequest request)
        {
            var result = _situacaoFinanceiraApplicationService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/finance-situation")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> SituacaoFinanceiraListar()
        {
            var result = _situacaoFinanceiraApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/finance-situation/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> SituacaoFinanceiraObterPorID([FromUri]int code)
        {
            var result = _situacaoFinanceiraApplicationService.ObterPorId(code);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/finance-situation/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> SituacaoFinanceiraDeletarPorId([FromUri] int code)
        {
            var result = _situacaoFinanceiraApplicationService.DeletarPorId(code);

            return CreateResponse(result);
        }
    }
}