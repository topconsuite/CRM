using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.TipoCobranca;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TipoCobrancaController : BaseController
    {
        private readonly ITipoCobrancaApplicationService _tipoCobrancaAppService;

        public TipoCobrancaController(ITipoCobrancaApplicationService tipoCobrancaAppService)
        {
            _tipoCobrancaAppService = tipoCobrancaAppService;
        }

        [HttpGet]
        [Route("v1/tiposCobranca/condicaoPagamento/{idCondicaoPagamento}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorCondicaoPagamento(int idCondicaoPagamento)
        {
            var tiposCobranca = _tipoCobrancaAppService.ListarPorCondicaoPagamento(idCondicaoPagamento);

            return CreateResponse(HttpStatusCode.OK, tiposCobranca);
        }

        [HttpGet]
        [Route("v1/tiposCobranca")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var tiposCobranca = _tipoCobrancaAppService.ListarTodos<TipoCobrancaResponse>();

            return CreateResponse(HttpStatusCode.OK, tiposCobranca);
        }
    }
}