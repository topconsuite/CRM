using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Request.ClicksignConfiguracao;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ClicksignConfiguracaoController : BaseController
    {
        private readonly IClicksignConfiguracaoApplicationService _clicksignConfiguracaoApplicationService;

        public ClicksignConfiguracaoController(IClicksignConfiguracaoApplicationService clicksignConfiguracaoApplicationService)
        {
            _clicksignConfiguracaoApplicationService = clicksignConfiguracaoApplicationService;
        }

        [HttpGet]
        [Route("v1/clicksign-configuracoes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var lista = _clicksignConfiguracaoApplicationService.ListarTodos();

            return CreateResponse(HttpStatusCode.OK, lista);
        }

        [HttpGet]
        [Route("v1/clicksign-configuracoes/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorId(int id)
        {
            var item = _clicksignConfiguracaoApplicationService.ObterPorId(id);

            return CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("v1/clicksign-configuracoes")]
        [Authorize]
        public Task<HttpResponseMessage> Salvar([FromBody] ClicksignConfiguracaoRequest request)
        {
            _clicksignConfiguracaoApplicationService.Salvar(request);

            return CreateResponse(HttpStatusCode.OK, "Salvo com sucesso!");
        }

        [HttpDelete]
        [Route("v1/clicksign-configuracoes/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> Remover(int id)
        {
            _clicksignConfiguracaoApplicationService.Remover(id);

            return CreateResponse(HttpStatusCode.OK, "Removido com sucesso!");
        }

        [HttpGet]
        [Route("v1/clicksign-configuracoes/{id}/usinas")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsinasPorConfiguracao(int id)
        {
            var lista = _clicksignConfiguracaoApplicationService.ListarUsinasPorConfiguracao(id);

            return CreateResponse(HttpStatusCode.OK, lista);
        }

        [HttpPost]
        [Route("v1/clicksign-configuracoes/{id}/usinas/{usinaId}")]
        [Authorize]
        public Task<HttpResponseMessage> VincularUsina(int id, int usinaId)
        {
            _clicksignConfiguracaoApplicationService.VincularUsina(id, usinaId);

            return CreateResponse(HttpStatusCode.OK, "Usina vinculada com sucesso!");
        }

        [HttpDelete]
        [Route("v1/clicksign-configuracoes/{id}/usinas/{usinaId}")]
        [Authorize]
        public Task<HttpResponseMessage> DesvincularUsina(int id, int usinaId)
        {
            _clicksignConfiguracaoApplicationService.DesvincularUsina(id, usinaId);

            return CreateResponse(HttpStatusCode.OK, "Usina desvinculada com sucesso!");
        }
    }
}
