using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TributacaoReformaController : BaseController
    {
        private readonly ITributacaoReformaApplicationService _tributacaoReformaAppService;

        public TributacaoReformaController(ITributacaoReformaApplicationService tributacaoReformaAppService)
        {
            _tributacaoReformaAppService = tributacaoReformaAppService;
        }

        [HttpGet]
        [Route("v1/tributacaoReforma/{imposto}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos(string imposto)
        {
            var listaTributacoes = _tributacaoReformaAppService.ListarTodosProducao(imposto);

            return CreateResponse(HttpStatusCode.OK, listaTributacoes);
        }
    }
}