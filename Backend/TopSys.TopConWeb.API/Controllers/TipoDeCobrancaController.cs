using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.TipoDeCobranca;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TipoDeCobrancaController : BaseController
    {
        private readonly ITipoDeCobrancaApplicationService _tipoDeCobrancaApplicationService;
        
        public TipoDeCobrancaController(ITipoDeCobrancaApplicationService tipoDeCobrancaApplicationService)
        {
            _tipoDeCobrancaApplicationService = tipoDeCobrancaApplicationService;
        }
        
        [HttpPost]
        [Route("integrations/billing-type")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDeCobrancaAdicionar([FromBody] TipoDeCobrancaRequest request)
        {
            var result = _tipoDeCobrancaApplicationService.Adicionar(request.TiposDeCobranca);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/billing-type/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDeCobrancaAtualizarPorId([FromUri] int code, [FromBody] TipoDeCobrancaAtualizarRequest request)
        {
            var result = _tipoDeCobrancaApplicationService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/billing-type")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDeCobrancaListar()
        {
            var result = _tipoDeCobrancaApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/billing-type/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDeCobrancaObterPorId([FromUri]int code)
        {
            var result = _tipoDeCobrancaApplicationService.ObterPorId(code);

            return CreateResponse(result);
        }
        

    }
}