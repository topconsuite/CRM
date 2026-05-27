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
using TopSys.TopConWeb.Application.DTOS.Request.Compromisso;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CompromissoController : BaseController
    {
        private readonly ICompromissoApplicationService _compromissoApplicationService;

        public CompromissoController(ICompromissoApplicationService compromissoApplicationService)
        {
            _compromissoApplicationService = compromissoApplicationService;
        }

        [HttpPost]
        [Route("v1/compromisso/grupo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarMultiplos([FromBody] CompromissoInclusaoRequest[] compromissos)
        {
            _compromissoApplicationService.AdicionarGrupo(compromissos, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Compromisso(s) adicionado(s) com sucesso");
        }

        [HttpPost]
        [Route("v1/compromisso")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] CompromissoInclusaoRequest compromisso)
        {
            compromisso.Usuario = User.Identity.Name; 
            _compromissoApplicationService.Adicionar(compromisso, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Compromisso adicionado com sucesso");
        }

        [HttpPatch]
        [Route("v1/compromisso")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] CompromissoAlteracaoRequest compromisso)
        {
            _compromissoApplicationService.Atualizar(compromisso, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Compromisso atualizada com sucesso!");
        }

        [HttpGet]
        [Route("v1/compromisso/usuarios/{idAgrupamento}")]
        [Authorize]
        public Task<HttpResponseMessage> UsuariosLigadosAgrupamento(string idAgrupamento)
        {

            var usuarios = _compromissoApplicationService.UsuariosLigadosAgrupamento(idAgrupamento);

            return CreateResponse(HttpStatusCode.OK, usuarios);

        }

        [HttpDelete]
        [Route("v1/compromisso/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _compromissoApplicationService.Deletar(codigo, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Compromisso deletado com sucesso!");

        }

        [HttpGet]
        [Route("v1/compromisso/usuarios")]
        [Authorize]
        public Task<HttpResponseMessage> ListarGrupoUsuario()
        {

            var usuarios = _compromissoApplicationService.ListarGrupoUsuario();

            return CreateResponse(HttpStatusCode.OK, usuarios);

        }

        [HttpGet]
        [Route("v1/compromisso/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorCodigo(int codigo)
        {
            var compromisso = _compromissoApplicationService.ObterPorId(codigo);

            return CreateResponse(HttpStatusCode.OK, compromisso);
        }

        [HttpGet]
        [Route("v1/compromisso/por-horario")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] CompromissoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<Compromisso>(request.Filter);

            if (request.FiltroUsuarios != null && request.FiltroUsuarios.Length > 0)
            {
                var filtrosUsuario = request.FiltroUsuarios.Split('|');
                urlFilter = urlFilter.And(t => filtrosUsuario.Contains(t.Usuario));
            }
            else
            {
                urlFilter = urlFilter.And(t => t.Usuario == User.Identity.Name);
            }

            if (!request?.Filter?.Contains("data") ?? true ) urlFilter = urlFilter.And(t => t.DataFim == DateTime.Today);

            if ((request?.filtroHoraInicioDe ?? "") != "")
            {
                var horarioDe = UrlFilterParser.ConvertHoraString(request.filtroHoraInicioDe);
                urlFilter = urlFilter.And(t => t.HoraInicio >= horarioDe);
            }

            if ((request?.filtroHoraInicioAte ?? "") != "")
            {
                var horarioAte = UrlFilterParser.ConvertHoraString(request.filtroHoraInicioAte);
                urlFilter = urlFilter.And(t => t.HoraInicio <= horarioAte);
            }

            if ((request?.filtroHoraFinalDe ?? "") != "")
            {
                var horarioDe = UrlFilterParser.ConvertHoraString(request.filtroHoraFinalDe);
                urlFilter = urlFilter.And(t => t.HoraFim >= horarioDe);
            }

            if ((request?.filtroHoraFinalAte ?? "") != "")
            {
                var horarioAte = UrlFilterParser.ConvertHoraString(request.filtroHoraFinalAte);
                urlFilter = urlFilter.And(t => t.HoraFim <= horarioAte);
            }

            var pagedList = _compromissoApplicationService.ListarEmOrdemDecrescentePorHorario(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }
    }
}