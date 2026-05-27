using LinqKit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Topsys.TopConWeb.SharedKernel.Filters;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoProduto;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class GrupoEconomicoController : BaseController
    {
        private readonly IGrupoEconomicoApplicationService _grupoEconomicoApplicationService;

        public GrupoEconomicoController(IGrupoEconomicoApplicationService grupoEconomicoApplicationService)
        {
            _grupoEconomicoApplicationService = grupoEconomicoApplicationService;
        }

        [HttpPost]
        [Route("v1/grupos-economicos")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] GrupoEconomicoInclusaoRequest request)
        {
            _grupoEconomicoApplicationService.Adicionar(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Grupo econômico Adicionado com Sucesso");
        }

        [HttpPatch]
        [Route("v1/grupos-economicos")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] GrupoEconomicoAlteracaoRequest request)
        {
            _grupoEconomicoApplicationService.Atualizar(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Grupo econômico Alterado com Sucesso");
        }

        [HttpDelete]
        [Route("v1/grupos-economicos/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _grupoEconomicoApplicationService.Deletar(codigo);

            return CreateResponse(HttpStatusCode.OK, "Grupo econômico deletado com Sucesso");
        }

        [HttpGet]
        [Route("v1/grupos-economicos")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] GrupoProdutoPagedRequest request)
        {
            if (request is null)
                return CreateResponse(HttpStatusCode.OK, _grupoEconomicoApplicationService.Listar());

            var urlFilter = UrlFilterParser.Convert<GrupoEconomicoFilter>(request.Filter);

            var pagedList = _grupoEconomicoApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/grupos-economicos/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Obter(int codigo)
        {
            var grupoEconomico = _grupoEconomicoApplicationService.ObterPorCodigo(codigo);

            return CreateResponse(HttpStatusCode.OK, grupoEconomico);
        }

    }
}