using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TesteController : BaseController
    {

        private readonly IComercialLegacyService _comercialLegacyService;

        
        public TesteController(IComercialLegacyService comercialLegacyService)
        {
            _comercialLegacyService = comercialLegacyService;
        }
        
        [HttpGet]
        [Route("v1/teste")]
        public Task<HttpResponseMessage> Teste()
        {
            var agAprov = "";
            var msgRetorno = "";

            //var teste = _comercialLegacyService.VerificarFraude(113532, "", 0, "", 0, "", 0, "", 0, 0, 0, out agAprov, out msgRetorno);


            throw new Exception("Minha Exception");


            return CreateResponse(HttpStatusCode.OK, null);
        }
    }
}