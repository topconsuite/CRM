using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Response.Slump;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class SlumpController : BaseController
    {
        private readonly ISlumpApplicationService _slumpApplicationService;

        public SlumpController(ISlumpApplicationService slumpApplicationService)
        {
            _slumpApplicationService = slumpApplicationService;
        }

        [HttpGet]
        [Route("v1/slumps")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaSlumps = _slumpApplicationService.ListarTodos<SlumpResponse>();

            return CreateResponse(HttpStatusCode.OK, listaSlumps);
        }

        [HttpGet]
        [Route("v1/slumps/slumpReal/{slumpReal}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorSlumpReal(int slumpReal)
        {
            var slumps = _slumpApplicationService.ListarPorSlumpReal(slumpReal);

            return CreateResponse(HttpStatusCode.OK, slumps);
        }
    }
}