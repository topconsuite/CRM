using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.Prensa;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class PrensaController : BaseController
    {
        private readonly IPrensaApplicationService _prensaAppService;

        public PrensaController(IPrensaApplicationService prensaAppService)
        {
            _prensaAppService = prensaAppService;
        }

        [HttpPost]
        [Route("integrations/press")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteAdicionar([FromBody][Required] PrensaRequest request)
        {
            var result = _prensaAppService.PrensaAdicionar(request);

            return CreateResponse(result);
        }
    }
}