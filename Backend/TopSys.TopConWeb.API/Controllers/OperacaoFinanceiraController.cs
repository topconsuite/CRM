using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.OperacaoFinanceira;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class OperacaoFinanceiraController : BaseController
    {
        private readonly IOperacaoFinanceiraApplicationService _operacaoFinanceiraApplicationService;

        public OperacaoFinanceiraController(IOperacaoFinanceiraApplicationService operacaoFinanceiraApplicationService)
        {
            _operacaoFinanceiraApplicationService = operacaoFinanceiraApplicationService;
        }
        
        [HttpPost]
        [Route("integrations/finance-operation")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> OperacaoFinanceiraAdicionar([FromBody] OperacaoFinanceiraRequest request)
        {
            var result = _operacaoFinanceiraApplicationService.Adicionar(request.TiposDeCobranca);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/finance-operation/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> OperacaoFinanceiraAtualizarPorId([FromUri] int code, [FromBody] OperacaoFinanceiraAtualizarRequest request)
        {
            var result = _operacaoFinanceiraApplicationService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/finance-operation")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> OperacaoFinanceiraListar()
        {
            var result = _operacaoFinanceiraApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/finance-operation/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> OperacaoFinanceiraObterPorId([FromUri]int code)
        {
            var result = _operacaoFinanceiraApplicationService.ObterPorId(code);

            return CreateResponse(result);
        }
        
        [HttpDelete]
        [Route("integrations/finance-operation/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> OperacaoFinanceiraDeletarPorId([FromUri] int code)
        {
            var result = _operacaoFinanceiraApplicationService.DeletarPorId(code);

            return CreateResponse(result);
        }
    }
}