using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.API.Helpers;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class VersionController : BaseController
    {
        [HttpGet]
        [Route("v1/versao")]
        public Task<HttpResponseMessage> Versao()
        {
            var versao = $"Versão gerada em: 2.1.{VersionHelper.topconApiDateVersion}/01";
            return CreateResponse(HttpStatusCode.OK, versao);
        }
    }
}