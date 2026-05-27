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
    public class ObraFrenteController : BaseController
    {

        private readonly IObraFrenteApplicationService _obraFrenteApplicationService;

        public ObraFrenteController(IObraFrenteApplicationService obraFrenteApplicationService)
        {
            _obraFrenteApplicationService = obraFrenteApplicationService;
        }

        [HttpGet]
        [Route("v1/obra-frente/{obraUsina}/{obraNumero}")]
        public Task<HttpResponseMessage> ListarPorObra(int obraUsina, int obraNumero)
        {
            var result = _obraFrenteApplicationService.ListarPorObra(obraUsina, obraNumero);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/obra-frente-em-uso/{obraUsina}/{obraNumero}/{obraSequencia}")]
        public Task<HttpResponseMessage> VerificarEnderecoPossuiProgramacao(int obraUsina, int obraNumero, int obraSequencia)
        {
            var result = _obraFrenteApplicationService.VerificarEnderecoPossuiProgramacao(obraUsina, obraNumero, obraSequencia);

            return CreateResponse(HttpStatusCode.OK, result);
        }

    }
}