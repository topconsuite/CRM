using LinqKit;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.Tarefa;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TarefaController: BaseController
    {
        private readonly ITarefaApplicationService _tarefaApplicationService;
        private readonly ICompromissoApplicationService _compromissoApplicationService;

        public TarefaController(ITarefaApplicationService tarefaApplicationService)
        {
            _tarefaApplicationService = tarefaApplicationService;
        }

        [HttpPost]
        [Route("v1/tarefa")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] TarefaInclusaoRequest tarefa)
        {
            tarefa.Usuario = User.Identity.Name;
            _tarefaApplicationService.Adicionar(tarefa, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Tarefa adicionado com sucesso");
        }

        [HttpPost]
        [Route("v1/tarefa/grupo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarMultiplos([FromBody] TarefaInclusaoRequest[] tarefas)
        {
            _tarefaApplicationService.AdicionarGrupo(tarefas, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Tarefa(s) adicionado(s) com sucesso");
        }

        [HttpPatch]
        [Route("v1/tarefa")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] TarefaAlteracaoRequest tarefa)
        {
            _tarefaApplicationService.Atualizar(tarefa, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Tarefa atualizada com sucesso!");
        }


        [HttpDelete]
        [Route("v1/tarefa/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _tarefaApplicationService.Deletar(codigo, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Tarefa deletado com sucesso!");

        }

        [HttpGet]
        [Route("v1/tarefa/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorCodigo(int codigo)
        {
            var tipoVisita = _tarefaApplicationService.ObterPorId(codigo);

            return CreateResponse(HttpStatusCode.OK, tipoVisita);
        }

        [HttpGet]
        [Route("v1/tarefa/usuarios/{idAgrupamento}")]
        [Authorize]
        public Task<HttpResponseMessage> UsuariosLigadosAgrupamento(string idAgrupamento)
        {

            var usuarios = _tarefaApplicationService.UsuariosLigadosAgrupamento(idAgrupamento);

            return CreateResponse(HttpStatusCode.OK, usuarios);

        }

        [HttpGet]
        [Route("v1/tarefa/por-horario")]
        [Authorize]
        public Task<HttpResponseMessage> ListarEmOrdemDecrescentePorHorario([FromUri] TarefaPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<Tarefa>(request.Filter);

            if(request.FiltroUsuarios != null && request.FiltroUsuarios.Length > 0)
            {
                var filtrosUsuario = request.FiltroUsuarios.Split('|');
                urlFilter = urlFilter.And(t => filtrosUsuario.Contains(t.Usuario));
            }
            else
            {
                urlFilter = urlFilter.And(t => t.Usuario == User.Identity.Name);
            }            

            if (!request?.Filter?.Contains("data") ?? true && !request.FiltroTarefasAtrasadas) urlFilter = urlFilter.And(t => t.Data == System.DateTime.Today);

            if (request.FiltroTarefasAtrasadas) urlFilter = urlFilter.And(t => t.Finalizado == false && t.Data < System.DateTime.Today);

            if (!request.FiltroTarefasAtrasadas && (!request?.Filter?.Contains("finalizado") ?? true)) urlFilter = urlFilter.And(t => t.Finalizado == false);

            if ((request?.FiltroHorarioDe ?? "") != "")
            {
                var horarioDe = UrlFilterParser.ConvertHoraString(request.FiltroHorarioDe);
                urlFilter = urlFilter.And(t => t.Horario >= horarioDe);
            }

            if ((request?.FiltroHorarioAte ?? "") != "")
            {
                var horarioAte = UrlFilterParser.ConvertHoraString(request.FiltroHorarioAte);
                urlFilter = urlFilter.And(t => t.Horario <= horarioAte);
            }

            var pagedList = _tarefaApplicationService.ListarEmOrdemDecrescentePorHorario(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }
    }
}