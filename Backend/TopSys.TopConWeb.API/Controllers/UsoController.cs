using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.Uso;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class UsoController : BaseController
    {
        private readonly IUsoApplicationService _usoApplicationService;

        public UsoController(IUsoApplicationService usoApplicationService)
        {
            _usoApplicationService = usoApplicationService;
        }

        [HttpGet]
        [Route("v1/usos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaUsos = _usoApplicationService.ListarTodos<UsoResponse>();

            return CreateResponse(HttpStatusCode.OK, listaUsos);
        }
    }
}