using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class UnidadeController : BaseController
    {
        private readonly IUnidadeApplicationService _unidadeApplicationService;

        public UnidadeController(IUnidadeApplicationService unidadeApplicationService)
        {
            _unidadeApplicationService = unidadeApplicationService;
        }

        [HttpGet]
        [Route("v1/unidades")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var unidadesList = _unidadeApplicationService.ListarTodos();

            return CreateResponse(HttpStatusCode.OK, unidadesList);
        }
    }
}