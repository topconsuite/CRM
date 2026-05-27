using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.PecaAConcretar;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class PecaAConcretarController : BaseController
    {
        private readonly IPecaAConcretarApplicationService _pecaAConcretarApplicationService;

        public PecaAConcretarController(IPecaAConcretarApplicationService pecaAConcretarApplicationService)
        {
            _pecaAConcretarApplicationService = pecaAConcretarApplicationService;
        }

        [HttpGet]
        [Route("v1/pecasAConcretar")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaPecasAConcretar = _pecaAConcretarApplicationService.ListarTodos<PecaAConcretarResponse>();

            return CreateResponse(HttpStatusCode.OK, listaPecasAConcretar);
        }
    }
}