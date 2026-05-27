using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.PortadorCobranca;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class PortadorController : BaseController
    {
        private readonly IPortadorApplicationService _portadorAppService;

        public PortadorController(IPortadorApplicationService portadorAppService)
        {
            _portadorAppService = portadorAppService;
        }

        [HttpGet]
        [Route("v1/portadores/vinculadosComContas")]
        [Authorize]
        public Task<HttpResponseMessage> ListarVinculadosComContas()
        {
            var portadores = _portadorAppService.ListarVinculadosComContas();

            return CreateResponse(HttpStatusCode.OK, portadores);
        }
        
        [HttpPost]
        [Route("integrations/bill-collector")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> PortadorAdicionar([FromBody] PortadorCobrancaRequest request)
        {
            var result = _portadorAppService.Adicionar(request.PortadoresCobranca);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/bill-collector/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> PortadorAtualizarPorId([FromUri] int code, [FromBody] PortadorCobrancaAtualizarRequest request)
        {
            var result = _portadorAppService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/bill-collector")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> PortadorListar()
        {
            var result = _portadorAppService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/bill-collector/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> PortadorObterPorId([FromUri]int code)
        {
            var result = _portadorAppService.ObterPorId(code);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/bill-collector/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> PortadorDeletarPorId([FromUri] int code)
        {
            var result = _portadorAppService.DeletarPorId(code);

            return CreateResponse(result);
        }
    }
}