using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.Filtered;
using TopSys.TopConWeb.Application.DTOS.Response.Mercadoria;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class MercadoriaController: BaseController
    {
        private readonly IMercadoriaApplicationService _mercadoriaApplicationService;

        public MercadoriaController(IMercadoriaApplicationService mercadoriaApplicationService)
        {
            _mercadoriaApplicationService = mercadoriaApplicationService;
        }

        [HttpGet]
        [Route("v1/mercadorias/produtos-e-servicos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarProdutosEServicos([FromUri] FilteredRequest request)
        {
            var filterLambda = UrlFilterParser.Parse<Mercadoria>(request != null ? request.Filter : null);

            var filter = filterLambda.And(t => t.ProdutoServico.Equals("P") || t.ProdutoServico.Equals("S") || t.ProdutoServico.Equals("M"));

            var mercadoriaList = _mercadoriaApplicationService.ListarFiltrados<MercadoriaResponse>(filter);

            return CreateResponse(HttpStatusCode.OK, mercadoriaList);
        }

        [HttpGet]
        [Route("v1/mercadorias/traco-desc-personalizado/uso/{idUso}/pedra/{idPedra}/slump/{idSlump}/resistenciaTipo/{idResistenciaTipo}/mpa/{mpa}/consumo/{consumo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterTracoMercadoriaComDescricaoPersonalizada( int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {

            var mercadoria = _mercadoriaApplicationService.ObterTracoMercadoriaComDescricaoPersonalizada(idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);

            return CreateResponse(HttpStatusCode.OK, mercadoria);
        }
    }
}