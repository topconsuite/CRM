using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco;
using TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco.Alteracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.API.Controllers
{

    [RoutePrefix("api")]
    public class PreTracoPrecoController : BaseController
    {

        private readonly IPreTracoPrecoApplicationService _preTracoPrecoApplicationService;

        public PreTracoPrecoController(IPreTracoPrecoApplicationService preTracoPrecoApplicationService)
        {
            _preTracoPrecoApplicationService = preTracoPrecoApplicationService;
        }

        [HttpGet]
        [Route("v1/preTracoPreco")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAguardandoCienciaPorPagina([FromUri] PreTracoPrecoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<PreTracoPreco>(request.Filter);

            var preTracoPrecos = _preTracoPrecoApplicationService.ListarAguardandoCienciaPorPagina(request.Pagina, request.PorPagina, request.Segmentacao, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, preTracoPrecos);
        }

        [HttpGet]
        [Route("v1/preTracoPreco/usina/{usina}/uso/{uso}/pedra/{pedra}/slump/{slump}/resistenciaTipo/{resistencia}/mpa/{mpa}/consumo/{consumo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterUltimoAguardandoCienciaPorTraco(int usina, int uso, int pedra, int slump, int resistencia, float mpa, int consumo)
        {

            var tracoPreco = _preTracoPrecoApplicationService.ObterUltimoAguardandoCienciaPorTraco(usina, uso, pedra, slump, resistencia, mpa, consumo);

            return CreateResponse(HttpStatusCode.OK, tracoPreco);

        }

        [HttpPatch]
        [Route("v1/preTracoPreco/ciente")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarConfirmarCiencia([FromBody] PreTracoPrecoAlteracaoRequest preTracoPrecoAlteracao)
        {

            _preTracoPrecoApplicationService.Atualizar(preTracoPrecoAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Alteração de preço confirmada com sucesso.");

        }

        [HttpPatch]
        [Route("v1/preTracoPreco/aprovar-todos")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarTodos([FromBody] IEnumerable<PreTracoPrecoAlteracaoRequest> preTracoPrecoAlteracao)
        {
            _preTracoPrecoApplicationService.AprovarTodos(preTracoPrecoAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Alteração de preço reprovada com sucesso.");
        }

        [HttpPatch]
        [Route("v1/preTracoPreco/reprovar")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarValorVenda([FromBody] PreTracoPrecoAlteracaoRequest preTracoPrecoAlteracao)
        {
            _preTracoPrecoApplicationService.Reprovar(preTracoPrecoAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Alteração de preço reprovada com sucesso.");
        }

        [HttpPatch]
        [Route("v1/preTracoPreco/reprovar-todos")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarTodos([FromBody] IEnumerable<PreTracoPrecoAlteracaoRequest> preTracoPrecoAlteracao)
        {
            _preTracoPrecoApplicationService.ReprovarTodos(preTracoPrecoAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Alteração de preço reprovada com sucesso.");
        }

        [HttpPatch]
        [Route("v1/preTracoPreco/atualizarLote")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarLote([FromBody] PreTracoPrecoAlteracaoLoteRequest preTracoPrecoAlteracao)
        {
            _preTracoPrecoApplicationService.AtualizarLote(preTracoPrecoAlteracao.Tracos, User.Identity.Name, preTracoPrecoAlteracao.Tipo, preTracoPrecoAlteracao.Valor);

            return CreateResponse(HttpStatusCode.OK, "Alteração em Lote confirmada com sucesso");

        }

    }
}