using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CadastroGeralController : BaseController
    {
        private readonly ICadastroGeralApplicationService _cadastroGeralAppService;

        public CadastroGeralController(ICadastroGeralApplicationService cadastroGeralAppService)
        {
            _cadastroGeralAppService = cadastroGeralAppService;
        }

        [HttpGet]
        [Route("v1/cadastroGeral/motivosBloqueioInterveniente")]
        [Authorize]
        public Task<HttpResponseMessage> ListarMotivosBloqueioInterveniente()
        {
            var motivosBloqueioInterveniente = _cadastroGeralAppService.ListarMotivosBloqueioInterveniente();

            return CreateResponse(HttpStatusCode.OK, motivosBloqueioInterveniente);
        }

        [HttpGet]
        [Route("v1/cadastroGeral/viasCaptacao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarViasCaptacao()
        {
            var viasCaptacao = _cadastroGeralAppService.ListarViasCaptacao();

            return CreateResponse(HttpStatusCode.OK, viasCaptacao);
        }

        [HttpGet]
        [Route("v1/cadastroGeral/tipo-obra")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTipoObra()
        {
            var tipoObra = _cadastroGeralAppService.ListarTipoObra();

            return CreateResponse(HttpStatusCode.OK, tipoObra);
        }

        [HttpGet]
        [Route("v1/cadastroGeral/porte-obra")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorteObra()
        {
            var porteObra = _cadastroGeralAppService.ListarPorteObra();

            return CreateResponse(HttpStatusCode.OK, porteObra);
        }

        [HttpGet]
        [Route("v1/cadastroGeral/funcoes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarFuncoes()
        {
            var funcoes = _cadastroGeralAppService.ListarFuncoes();

            return CreateResponse(HttpStatusCode.OK, funcoes);
        }

        [HttpGet]
        [Route("v1/cadastroGeral/temposAprovacaoMedicao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTemposAprovacaoMedicao()
        {
            var temposAprovacao = _cadastroGeralAppService.ListarTemposAprovacaoMedicao();

            return CreateResponse(HttpStatusCode.OK, temposAprovacao);
        }
    }
}