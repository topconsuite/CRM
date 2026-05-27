using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class OportunidadeController : BaseController
    {
        private readonly IOportunidadeTipoApplicationService _oportunidadeTipoApplicationService;
        private readonly IConcorrenteApplicationService _concorrenteApplicationService;
        private readonly IOportunidadeApplicationService _oportunidadeApplicationService;

        public OportunidadeController(
            IOportunidadeTipoApplicationService oportunidadeTipoApplicationService, 
            IConcorrenteApplicationService concorrenteApplicationService,
            IOportunidadeApplicationService oportunidadeApplicationService)
        {
            _oportunidadeTipoApplicationService = oportunidadeTipoApplicationService;
            _concorrenteApplicationService = concorrenteApplicationService;
            _oportunidadeApplicationService = oportunidadeApplicationService;
        }

        #region Oportunidade Tipo

        [HttpPost]
        [Route("v1/oportunidade/tipo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarOportunidadeTipo([FromBody] OportunidadeTipoInclusaoRequest oportunidadeTipo)
        {
            _oportunidadeTipoApplicationService.Adicionar(oportunidadeTipo, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Tipo de Oportunidade adicionado com sucesso");
        }

        [HttpPatch]
        [Route("v1/oportunidade/tipo")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarOportunidadeTipo([FromBody] OportunidadeTipoAlteracaoRequest oportunidadeTipo)
        {
            _oportunidadeTipoApplicationService.Atualizar(oportunidadeTipo, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Tipo de Oportunidade atualizada com sucesso!");
        }


        [HttpDelete]
        [Route("v1/oportunidade/tipo/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> DeletarOportunidadeTipo(int codigo)
        {
            _oportunidadeTipoApplicationService.Deletar(codigo, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Tipo de Oportunidade deletado com sucesso!");

        }

        [HttpGet]
        [Route("v1/oportunidade/tipo/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterOportunidadeTipoPorCodigo(int codigo)
        {
            var oportunidadeTipo = _oportunidadeTipoApplicationService.ObterPorCodigo(codigo);

            return CreateResponse(HttpStatusCode.OK, oportunidadeTipo);
        }

        [HttpGet]
        [Route("v1/oportunidade/tipos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarOportunidadeTipos([FromUri] OportunidadeTipoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<OportunidadeTipo>(request.Filter);

            var pagedList = _oportunidadeTipoApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/oportunidade/tipos-ativos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTiposAtivos()
        {
            var fases = _oportunidadeApplicationService.ListarTiposAtivos();

            return CreateResponse(HttpStatusCode.OK, fases);
        }

        #endregion

        #region Concorrente

        [HttpPost]
        [Route("v1/oportunidade/concorrente")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarConcorrente([FromBody] ConcorrenteInclusaoRequest concorrente)
        {
            _concorrenteApplicationService.Adicionar(concorrente, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Concorrente adicionado com sucesso");
        }

        [HttpPatch]
        [Route("v1/oportunidade/concorrente")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarConcorrente([FromBody] ConcorrenteAlteracaoRequest concorrente)
        {
            _concorrenteApplicationService.Atualizar(concorrente, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Concorrente atualizada com sucesso!");
        }


        [HttpDelete]
        [Route("v1/oportunidade/concorrente/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> DeletarConcorrente(int codigo)
        {
            _concorrenteApplicationService.Deletar(codigo, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Concorrente deletado com sucesso!");

        }

        [HttpGet]
        [Route("v1/oportunidade/concorrente/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterConcorrentePorCodigo(int codigo)
        {
            var concorrente = _concorrenteApplicationService.ObterPorCodigo(codigo);

            return CreateResponse(HttpStatusCode.OK, concorrente);
        }

        [HttpGet]
        [Route("v1/oportunidade/concorrentes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarConcorrentes([FromUri] ConcorrentePagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<Concorrente>(request.Filter);

            var pagedList = _concorrenteApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/oportunidade/concorrentes-ativos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarConcorrentesAtivos()
        {

            var concorrentes = _oportunidadeApplicationService.ListarConcorrentesAtivos();

            return CreateResponse(HttpStatusCode.OK, concorrentes);

        }

        #endregion

        #region Oportunidade

        [HttpGet]
        [Route("v1/oportunidade")]
        [Authorize]
        public Task<HttpResponseMessage> ListarEmOrdemDecrescente([FromUri] OportunidadePagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<Oportunidade>(request.Filter);
            var pagedList = _oportunidadeApplicationService.ListarEmOrdemDecrescente(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpPost]
        [Route("v1/oportunidade")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] OportunidadeAdicionarRequest request)
        {
            var response = _oportunidadeApplicationService.Adicionar(request);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPatch]
        [Route("v1/oportunidade")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] OportunidadeAtualizarRequest request)
        {
            _oportunidadeApplicationService.Atualizar(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Atualizado com sucesso.");
        }

        [HttpGet]
        [Route("v1/oportunidade/usina/{usina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorId([FromUri] int usina, int ano, int numero)
        {
            var response = _oportunidadeApplicationService.ObterPorId(usina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/oportunidade/usina/{usina}/ano/{ano}/numero/{numero}/gerar-proposta")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPropostaDeOportunidade(int usina, int ano, int numero)
        {
            var proposta = _oportunidadeApplicationService.ObterPropostaDeOportunidade(usina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, proposta);
        }

        #endregion

        #region Fases

        [HttpGet]
        [Route("v1/oportunidade/fases")]
        [Authorize]
        public Task<HttpResponseMessage> ListarFases()
        {
            var fases = _oportunidadeApplicationService.ListarFases();

            return CreateResponse(HttpStatusCode.OK, fases);
        }


        #endregion


        [HttpGet]
        [Route("v1/oportunidade/interacoes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarInteracoes([FromUri] OportunidadeInteracaoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<OportunidadeInteracao>(request.Filter);

            var interacoes = _oportunidadeApplicationService.ListarInteracoes(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, interacoes);
        }

        [HttpPost]
        [Route("v1/oportunidade/interacao")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarInteracao([FromBody] OportunidadeInteracaoAdicionarRequest interacao)
        {
            _oportunidadeApplicationService.AdicionarInteracao(User.Identity.Name, interacao);

            return CreateResponse(HttpStatusCode.OK, "Interação adicionada com Sucesso");
        }
    }
}