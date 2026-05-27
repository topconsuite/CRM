using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Request.Usuario;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioApplicationService _usuarioApplicationService;

        public UsuarioController(IUsuarioApplicationService usuarioService)
        {
            _usuarioApplicationService = usuarioService;
        }

        
        [HttpPost]
        [Route("v1/usuario")]
        public Task<HttpResponseMessage> Registrar(RegistrarUsuarioRequest usuario)
        {
            var registroUsuario =  _usuarioApplicationService.Registrar(usuario);
            return CreateResponse(HttpStatusCode.OK, registroUsuario);
        }

        [HttpPost]
        [Route("v1/usuario/cadastrar/senha")]
        public Task<HttpResponseMessage> CadastrarSenha([FromBody]CadastrarSenhaUsuarioRequest usuario)
        {
            var cadastrarSenhaUsuario = _usuarioApplicationService.CadastrarSenhaUsuario(usuario);
            return CreateResponse(HttpStatusCode.OK, cadastrarSenhaUsuario);
        }

        [HttpPost]
        [Route("v1/usuario/acesso-aplicacao/aplicativo/{aplicativo}/programa/{programa}")]
        public Task<HttpResponseMessage> GravarAcessoAplicacao(string aplicativo, int programa)
        {
            _usuarioApplicationService.GravarAcessoAplicacao(User.Identity.Name, aplicativo, programa);
            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/usuario/filtro/aplicativo/{aplicativo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterFiltroWebPorId(string aplicativo)
        {
            var usuario = User.Identity.Name;
            var result = _usuarioApplicationService.ObterFiltroWebPorId(usuario, aplicativo);
            return CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpPost]
        [Route("v1/usuario/filtro")]
        [Authorize]
        public Task<HttpResponseMessage> SalvarWebFiltro(UsuarioWebFiltroSalvarRequest request)
        {
            var usuario = User.Identity.Name;
            _usuarioApplicationService.SalvarFiltroWeb(usuario, request);
            return CreateResponse(HttpStatusCode.OK, "Filtro salvo com sucesso.");
        }

    }
}