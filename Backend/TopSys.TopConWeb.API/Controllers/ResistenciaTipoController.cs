using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ResistenciaTipoController : BaseController
    {
        private readonly IResistenciaTipoApplicationService _resistenciaTipoApplicationService;

        public ResistenciaTipoController(IResistenciaTipoApplicationService resistenciaTipoApplicationService)
        {
            _resistenciaTipoApplicationService = resistenciaTipoApplicationService;
        }

        [HttpGet]
        [Route("v1/resitenciaTipos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaResistencias = _resistenciaTipoApplicationService.ListarTodos<ResistenciaTipo>();

            return CreateResponse(HttpStatusCode.OK, listaResistencias);
        }
    }
}