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
    public class ParametroController : BaseController
    {
        private readonly IParametroApplicationService _parametroAppService;

        public ParametroController(IParametroApplicationService parametroAppService)
        {
            _parametroAppService = parametroAppService;
        }

        [HttpGet]
        [Route("v1/parametro-proposta/data-base/{dataBase}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterParametroPropostaPorDataBase(DateTime dataBase)
        {
            var parametro = _parametroAppService.ObterPorDataBase<ParametroProposta>(dataBase);

            return CreateResponse(HttpStatusCode.OK, parametro);
        }

        [HttpGet]
        [Route("v1/parametro/group/{group}/key/{key}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterParametroN(string group, string key)
        {
            var parametro = _parametroAppService.ObterParametroN(group, key);

            return CreateResponse(HttpStatusCode.OK, parametro);
        }

        [HttpGet]
        [Route("v1/parametro/group/{group}/key/{key}/value/{value}")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarParametroN(string group, string key, string value)
        {
            _parametroAppService.AtualizarParametroN(group, key, value);

            return CreateResponse(HttpStatusCode.OK, "Atualizado com sucesso!");
        }
    }
}