using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Response.CartaoBandeira;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CartaoBandeiraController : BaseController
    {
        private readonly ICartaoBandeiraApplicationService _cartaoBandeiraAppService;

        public CartaoBandeiraController(ICartaoBandeiraApplicationService cartaoBandeiraAppService)
        {
            _cartaoBandeiraAppService = cartaoBandeiraAppService;
        }

        [HttpGet]
        [Route("v1/cartaoBandeiras")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var cartaoBandeiras = _cartaoBandeiraAppService.ListarTodosComAgregados();

            return CreateResponse(HttpStatusCode.OK, cartaoBandeiras);
        }

        [HttpGet]
        [Route("v1/cartaoBandeiras/com-integracao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarComIntegracao()
        {
            var cartaoBandeiras = _cartaoBandeiraAppService.ListarTodosComIntegracao();

            return CreateResponse(HttpStatusCode.OK, cartaoBandeiras);
        }
    }
}