using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.LiberacaoAcesso;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class LiberacaoAcessoController : BaseController
    {
        private readonly ILiberacaoAcessoApplicationService _liberacaoAcessoApplicationService;

        public LiberacaoAcessoController(ILiberacaoAcessoApplicationService liberacaoAcessoApplicationService)
        {
            _liberacaoAcessoApplicationService = liberacaoAcessoApplicationService;
        }

        [HttpPost]
        [Route("v1/liberacoes-acessos/grupo")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] GrupoAcessoInclusaoRequest request)
        {
            _liberacaoAcessoApplicationService.Adicionar(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Grupo Adicionado com Sucesso");
        }

        [HttpPatch]
        [Route("v1/liberacoes-acessos/grupo/{alteraUsuarios}")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] GrupoAcessoAlteracaoRequest request, bool alteraUsuarios)
        {
            _liberacaoAcessoApplicationService.Atualizar(request, alteraUsuarios, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Grupo Alterado com Sucesso");
        }

        [HttpDelete]
        [Route("v1/liberacoes-acessos/grupo/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _liberacaoAcessoApplicationService.Deletar(codigo, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Grupo Deletado com Sucesso");
        }

        [HttpGet]
        [Route("v1/liberacoes-acessos/grupos")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] GrupoAcessoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<GrupoAcesso>(request.Filter);

            var pagedList = _liberacaoAcessoApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/liberacoes-acessos/grupo/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterGrupo(int codigo)
        {
            var grupoAcesso = _liberacaoAcessoApplicationService.ObterGrupoPorCodigo(codigo);

            return CreateResponse(HttpStatusCode.OK, grupoAcesso);
        }

        // --------------------------------------------------------- Usuários ----------------------------------------------------------------

        [HttpPost]
        [Route("v1/liberacoes-acessos/usuarios/{codigoGrupoAcesso}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsuariosPorGrupoAcesso(int codigoGrupoAcesso)
        {
            var result = _liberacaoAcessoApplicationService.ListarUsuariosPorGrupoAcesso(codigoGrupoAcesso);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/liberacoes-acessos/usuario/ausencia/{usuario}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPeriodosAusenciaPorUsuario(string usuario)
        {
            var result = _liberacaoAcessoApplicationService.ListarPeriodosAusenciaPorUsuario(usuario);

            return CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpPost]
        [Route("v1/liberacoes-acessos/usuarios-disponiveis")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsuariosDisponiveis()
        {
            var result = _liberacaoAcessoApplicationService.ListarUsuarios();

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/liberacoes-acessos/usuario")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarUsuario(IEnumerable<LiberacaoAcessoInclusaoRequest> liberacaoAcessoUsuarioInclusaoRequest)
        {

            var result = _liberacaoAcessoApplicationService.AdicionarUsuario(liberacaoAcessoUsuarioInclusaoRequest, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost]
        [Route("v1/liberacoes-acessos/usuario/ausencia")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarPeriodoAusenciaUsuario(IEnumerable<PeriodoAusenciaUsuarioAlteracaoRequest> periodoAusenciaUsuarioAlteracaoRequest)
        {
            _liberacaoAcessoApplicationService.AtualizarPeriodoAusenciaUsuario(periodoAusenciaUsuarioAlteracaoRequest, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "");

        }

        [HttpPatch]
        [Route("v1/liberacoes-acessos/usuario")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarUsuario([FromBody] IEnumerable<LiberacaoAcessoAlteracaoRequest> request)
        {
            _liberacaoAcessoApplicationService.AtualizarUsuario(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Usuario Alterado com Sucesso");
        }

        [HttpDelete]
        [Route("v1/liberacoes-acessos/usuario/remover/{usuarioCodigo}")]
        [Authorize]
        public Task<HttpResponseMessage> RemoverUsuario(string usuarioCodigo)
        {

            _liberacaoAcessoApplicationService.RemoverUsuario(usuarioCodigo, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Removido com sucesso.");

        }

        [HttpGet]
        [Route("v1/liberacoes-acessos/usuario")]
        [Authorize]
        public Task<HttpResponseMessage> ObterLiberacaoAcessoUsuario()
        {
            var possuiAcesso = _liberacaoAcessoApplicationService.ObterLiberacaoAcessoUsuario(User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, possuiAcesso);
        }
    }
}