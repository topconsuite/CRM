using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.TipoDocumento;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TipoDocumentoController : BaseController
    {
        private readonly ITipoDocumentoApplicationService _tipoDocumentoApplicationService;
        
        public TipoDocumentoController(ITipoDocumentoApplicationService tipoDocumentoApplicationService)
        {
            _tipoDocumentoApplicationService = tipoDocumentoApplicationService;
        }
        
        [HttpPost]
        [Route("integrations/document-type")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDocumentoAdicionar([FromBody] TipoDocumentoRequest request)
        {
            var result = _tipoDocumentoApplicationService.Adicionar(request.TiposDocumento);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/document-type/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDocumentoAtualizarPorId([FromUri] int code, [FromBody] TipoDocumentoAtualizarRequest request)
        {
            var result = _tipoDocumentoApplicationService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/document-type")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDocumentoListar()
        {
            var result = _tipoDocumentoApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/document-type/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TipoDocumentoObterPorId([FromUri]int code)
        {
            var result = _tipoDocumentoApplicationService.ObterPorId(code);

            return CreateResponse(result);
        }
    }
}