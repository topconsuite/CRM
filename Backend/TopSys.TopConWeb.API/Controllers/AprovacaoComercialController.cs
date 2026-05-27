using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Request.AprovacaoComercial;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;

namespace TopSys.TopConWeb.API.Controllers
{

    [RoutePrefix("api")]
    public class AprovacaoComercialController : BaseController
    {

        private readonly IAprovacaoComercialApplicationService _aprovacaoComercialApplicationService;
        private readonly IAprovacaoComercialHierarquiaApplicationService _aprovacaoComercialHierarquiaApplicationService;

        public AprovacaoComercialController(
            IAprovacaoComercialApplicationService aprovacaoComercialApplicationService,
            IAprovacaoComercialHierarquiaApplicationService aprovacaoComercialHierarquiaApplicationService)
        {
            _aprovacaoComercialApplicationService = aprovacaoComercialApplicationService;
            _aprovacaoComercialHierarquiaApplicationService = aprovacaoComercialHierarquiaApplicationService;
        }

        [HttpGet]
        [Route("v1/aprovacao-comercial")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAprovacaoComercialUsina([FromUri] AprovacaoComercialUsinaPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<AprovacaoComercialUsina>(request.Filter);

            var aprovacaoComercialUsinas = _aprovacaoComercialApplicationService.ListarAprovacaoComercialUsina(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, aprovacaoComercialUsinas);
        }


        [HttpPatch]
        [Route("v1/aprovacao-comercial/atualizar")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] AprovacaoComercialUsinaAlteracaoRequest request)
        {
            _aprovacaoComercialApplicationService.Atualizar(request);

            return CreateResponse(HttpStatusCode.OK, "Atualizado com sucesso.");
        }

        [HttpPost]
        [Route("v1/aprovacao-comercial/adicionar")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] AprovacaoComercialUsinaInsercaoRequest request)
        {
            var result = _aprovacaoComercialApplicationService.Adicionar(request);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        // --------------------------------------------------------- Hierarquia ----------------------------------------------------------------

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/listar-por-usina/{usinaId}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarHierarquiaPorUsina(Guid usinaId)
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.ListarNivelHierarquiaPorUsina(usinaId);

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/proximo-nivel-autoridade/{aprovacaoComercialUsinaId}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterProximoNivelHierarquiaPorAprovacaoComercialUsina(string aprovacaoComercialUsinaId)
        {
            var result = _aprovacaoComercialHierarquiaApplicationService.ObterProximoNivelHierarquiaPorAprovacaoComercialUsina(new Guid(aprovacaoComercialUsinaId));

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/adicionar")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarHierarquia(AprovacaoComercialHierarquiaInsercaoRequest aprovacaoComercialHierarquiaInsercaoRequest)
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.AdicionarHierarquia(aprovacaoComercialHierarquiaInsercaoRequest);

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPatch]
        [Route("v1/aprovacao-comercial-hierarquia/atualizar")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarHierarquia(AprovacaoComercialHierarquiaAlteracaoRequest aprovacaoComercialHierarquiaAlteracaoRequest)
        {

            _aprovacaoComercialHierarquiaApplicationService.AtualizarHierarquia(aprovacaoComercialHierarquiaAlteracaoRequest);

            return CreateResponse(HttpStatusCode.OK, "Atualizado com sucesso.");

        }

        // --------------------------------------------------------- Usuários ----------------------------------------------------------------

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/usuarios/{hierarquiaId}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsuarioPorHierarquia(string hierarquiaId)
        {
            var result = _aprovacaoComercialHierarquiaApplicationService.ListarUsuariosPorNivelHierarquia(new Guid(hierarquiaId));

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/usuarios-disponiveis")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsuariosDisponiveis()
        {
            var result = _aprovacaoComercialHierarquiaApplicationService.ListarUsuarios();

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/usuario")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarUsuario(AprovacaoComercialHierarquiaUsuarioInsercaoRequest aprovacaoComercialHierarquiaUsuarioInsercaoRequest)
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.AdicionarUsuario(aprovacaoComercialHierarquiaUsuarioInsercaoRequest);

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpDelete]
        [Route("v1/aprovacao-comercial-hierarquia/usuario/remover/{aproUsuarioId}")]
        [Authorize]
        public Task<HttpResponseMessage> RemoverUsuario(string aproUsuarioId)
        {

            _aprovacaoComercialHierarquiaApplicationService.RemoverUsuario(new Guid(aproUsuarioId));

            return CreateResponse(HttpStatusCode.OK, "Removido com sucesso.");

        }

        // -------------------- Condições ----------------------------------

        [HttpGet]
        [Route("v1/aprovacao-comercial-hierarquia/tipo-pessoa")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTipoPessoa()
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.ListarTipoPessoa();

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpGet]
        [Route("v1/aprovacao-comercial-hierarquia/condicao/nivel-hierarquia/{aprovComercialHierarquiaId}/tipo-pessoa/{tipoPessoaId}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarCondicoesPorHierarquia(string aprovComercialHierarquiaId, string tipoPessoaId)
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.ListarCondicoesPorNivelHierarquiaTipoPessoa(new Guid(aprovComercialHierarquiaId), new Guid(tipoPessoaId));

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/condicao/adicionar-lote")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarCondicoesEmLote([FromBody] AprovacaoComercialHierarquiaCondicaoInsercaoRequest[] condicoes)
        {

            _aprovacaoComercialHierarquiaApplicationService.AdicionarCondicoes(condicoes);

            return CreateResponse(HttpStatusCode.OK, "Adicionado(s) com sucesso.");

        }

        // -------------------- Demais Condições ----------------------------------


        [HttpGet]
        [Route("v1/aprovacao-comercial-hierarquia/demais-condicao/nivel-hierarquia/{aprovComercialHierarquiaId}/tipo-pessoa/{tipoPessoaId}/segmentacao/{segmentacaoId}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterCondicaPagamentoPorHierarquiaTipoPessoaSegmentacao(string aprovComercialHierarquiaId, string tipoPessoaId, int segmentacaoId)
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.ObterCondicaPagamentoPorHierarquiaTipoPessoaSegmentacao(new Guid(aprovComercialHierarquiaId), new Guid(tipoPessoaId), segmentacaoId);

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost]
        [Route("v1/aprovacao-comercial-hierarquia/demais-condicao")]
        public Task<HttpResponseMessage> AtualizarCondicaoPagamento([FromBody] AprovacaoComercialCondicaoPagamentoAtualizarRequest[] request)
        {

            var result = _aprovacaoComercialHierarquiaApplicationService.AtualizarCondicaoPagamento(request);

            return CreateResponse(HttpStatusCode.OK, result);

        }

        // ----------------------------------------------------------------------

        [HttpGet]
        [Route("v1/aprovacao-comercial/direitos-usuario/{obraUsina}/{obraNumero}/{obraVersao}")]
        [Authorize]
        public Task<HttpResponseMessage> UsuarioTemDireitoAprovacaoObra(int obraUsina, int obraNumero, int obraVersao)
        {
            var userId = User.Identity.Name;

            var result = _aprovacaoComercialHierarquiaApplicationService.UsuarioTemDireitoAprovacaoComercialObra(userId, obraUsina, obraNumero, obraVersao);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/aprovacao-comercial/listar-aprovacoes/{obraUsina}/{obraNumero}/{obraVersao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAprovacoes(int obraUsina, int obraNumero, int obraVersao)
        {
            var result = _aprovacaoComercialHierarquiaApplicationService.ListarDadosAprovacoesComercialObra(obraUsina, obraNumero, obraVersao);

            return CreateResponse(HttpStatusCode.OK, result);
        }

    }
}