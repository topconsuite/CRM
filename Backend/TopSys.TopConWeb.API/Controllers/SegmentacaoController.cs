using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.Segmentacao;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class SegmentacaoController : BaseController
    {
        private readonly ISegmentacaoApplicationService _segmentacaoApplicationService;

        public SegmentacaoController(ISegmentacaoApplicationService segmentacaoApplicationService)
        {
            _segmentacaoApplicationService = segmentacaoApplicationService;
        }

        [HttpGet]
        [Route("v1/segmentacao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaSegmentacao = _segmentacaoApplicationService.ListarTodos<SegmentacaoResponse>();

            return CreateResponse(HttpStatusCode.OK, listaSegmentacao);
        }

    }
}




