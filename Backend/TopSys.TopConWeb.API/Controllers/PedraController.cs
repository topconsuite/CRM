using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.Pedra;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class PedraController : BaseController
    {
        private readonly IPedraApplicationService _pedraApplicationService;

        public PedraController(IPedraApplicationService pedraApplicationService)
        {
            _pedraApplicationService = pedraApplicationService;
        }

        [HttpGet]
        [Route("v1/pedras")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaPedras = _pedraApplicationService.ListarTodos<PedraResponse>();

            return CreateResponse(HttpStatusCode.OK, listaPedras);
        }

    }
}