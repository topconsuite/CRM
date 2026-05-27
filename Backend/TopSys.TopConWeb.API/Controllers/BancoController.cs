using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.Banco;
using TopSys.TopConWeb.API.Security;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class BancoController : BaseController
    {
        private readonly IBancoApplicationService _bancoApplicationService;

        public BancoController(IBancoApplicationService bancoApplicationService)
        {
            _bancoApplicationService = bancoApplicationService;
        }

        [HttpPost]
        [Route("integrations/bank")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> BancoAdicionar([FromBody] BancoRequest request)
        {
            var result = _bancoApplicationService.Adicionar(request.Bancos);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/bank/{code}/{company}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> BancoAtualizarPorId([FromUri] int code, [FromUri] int company, [FromBody] BancoAtualizarRequest request)
        {
            var result = _bancoApplicationService.AtualizarPorId(code, company, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/bank")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> BancoListar()
        {
            var result = _bancoApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/bank/{code}/{company}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> BancoObterPorID([FromUri] int code, [FromUri] int company)
        {
            var result = _bancoApplicationService.ObterPorId(code, company);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/bank/{code}/{company}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> BancoDeletarPorId([FromUri] int code, [FromUri] int company)
        {
            var result = _bancoApplicationService.DeletarPorId(code, company);

            return CreateResponse(result);
        }
    }
}