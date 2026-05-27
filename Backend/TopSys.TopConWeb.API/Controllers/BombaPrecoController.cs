using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class BombaPrecoController : BaseController
    {
        private readonly IBombaPrecoApplicationService _bombaPrecoAppService;

        public BombaPrecoController(IBombaPrecoApplicationService bombaPrecoAppService)
        {
            _bombaPrecoAppService = bombaPrecoAppService;
        }

        [HttpGet]
        [Route("v1/bombaPreco/bombaTipos/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarBombaTiposPorUsina(int idUsina)
        {
            var bombaTipos = _bombaPrecoAppService.ListarBombaTiposPorUsina(idUsina);

            return CreateResponse(HttpStatusCode.OK, bombaTipos);
        }

        [HttpGet]
        [Route("v1/bombaPreco/terceiros/bombaTipo/{idBombaTipo}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTerceirosPorBombaTipo(int idBombaTipo)
        {
            var terceiros = _bombaPrecoAppService.ListarTerceirosPorBombaTipo(idBombaTipo);

            return CreateResponse(HttpStatusCode.OK, terceiros);
        }

        [HttpGet]
        [Route("v1/bombaPreco/usina/{idUsina}/bombaTipo/{idBombaTipo}/data/{data}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorUsinaBombaTipoData(int idUsina, int idBombaTipo, DateTime data)
        {
            var bombaPreco = _bombaPrecoAppService.ObterPorUsinaBombaTipoData(idUsina, idBombaTipo, data);

            return CreateResponse(HttpStatusCode.OK, bombaPreco);
        }
        
        [HttpGet]
        [Route("v1/bombaPreco/terceiro/{idTerceiro}/bombaTipo/{idBombaTipo}/data/{data}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorBombistaBombaTipoData(int idTerceiro, int idBombaTipo, DateTime data)
        {
            var bombaPreco = _bombaPrecoAppService.ObterPorBombistaBombaTipoData(idTerceiro, idBombaTipo, data);

            return CreateResponse(HttpStatusCode.OK, bombaPreco);
        }

        [HttpGet]
        [Route("v1/bombaPreco/usina/{idUsina}/bombaTipo/{idBombaTipo}/distancia-tubulacao/{distanciaTubulacao}/valor-adicional")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorAdicional(int idUsina, int idBombaTipo, int distanciaTubulacao)
        {
            var valorAdicional = _bombaPrecoAppService.ObterValorAdicional(idUsina, idBombaTipo, distanciaTubulacao);

            return CreateResponse(HttpStatusCode.OK, valorAdicional);
        }
    }
}