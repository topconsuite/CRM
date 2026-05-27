using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.Visita;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Infra.Reports;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class VisitaController : BaseController
    {
        private readonly IVisitaTipoApplicationService _tipoVisitaApplicationService;
        private readonly IVisitaApplicationService _visitaApplicationService;
        private readonly IIntervenienteApplicationService _intervenienteApplicationService;

        public VisitaController(
            IVisitaTipoApplicationService tipoVisitaApplicationService,
            IVisitaApplicationService visitaApplicationService,
            IIntervenienteApplicationService intervenienteApplicationService)
        {
            _tipoVisitaApplicationService = tipoVisitaApplicationService;
            _visitaApplicationService = visitaApplicationService;
            _intervenienteApplicationService = intervenienteApplicationService;
        }

        [HttpGet]
        [Route("v1/visita/tipo/ativos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTipoVisitaAtivo()
        {
            return CreateResponse(HttpStatusCode.OK, _tipoVisitaApplicationService.ListarTipoVisitasAtivas());
        }

        [HttpPost]
        [Route("v1/visita/tipo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarTipoVisita([FromBody] VisitaTipoInclusaoRequest tipoVisita)
        {
            _tipoVisitaApplicationService.Adicionar(tipoVisita, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Tipo de Visita adicionado com sucesso");
        }

        [HttpPatch]
        [Route("v1/visita/tipo")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarTipoVisita([FromBody] VisitaTipoAlteracaoRequest tipoVisita)
        {
            _tipoVisitaApplicationService.Atualizar(tipoVisita, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Tipo de Visita atualizada com sucesso!");
        }


        [HttpDelete]
        [Route("v1/visita/tipo/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> DeletarTipoVisita(int codigo)
        {
            _tipoVisitaApplicationService.Deletar(codigo, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Tipo de Visita deletado com sucesso!");

        }

        [HttpGet]
        [Route("v1/visita/tipo/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterTipoVisitaPorCodigo(int codigo)
        {
            var tipoVisita = _tipoVisitaApplicationService.ObterPorCodigo(codigo);

            return CreateResponse(HttpStatusCode.OK, tipoVisita);
        }

        [HttpGet]
        [Route("v1/visita/tipos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTiposVisita([FromUri] VisitaTipoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<VisitaTipo>(request.Filter);

            var pagedList = _tipoVisitaApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        // ---------------------------------- Visita ---------------------------------------------

        [HttpGet]
        [Route("v1/visita/usina/{usina}/ano/{ano}/numero/{numero}/gerar-lead")]
        [Authorize]
        public Task<HttpResponseMessage> ObterLeadDeVisita(int usina, int ano, int numero)
        {
            var lead = _visitaApplicationService.ObterLeadDeVisita(usina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, lead);
        }

        [HttpGet]
        [Route("v1/visita")]
        [Authorize]
        public Task<HttpResponseMessage> ListarEmOrdemDecrescente([FromUri] VisitaPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<Visita>(request.Filter);
            var pagedList = _visitaApplicationService.ListarEmOrdemDecrescente(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpPost]
        [Route("v1/visita")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] VisitaAdicionarRequest request)
        {
            var response = _visitaApplicationService.Adicionar(request);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPatch]
        [Route("v1/visita")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] VisitaAtualizarRequest request)
        {
            _visitaApplicationService.Atualizar(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Atualizado com sucesso.");
        }

        [HttpGet]
        [Route("v1/visita/usina/{usina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorId([FromUri] int usina, int ano, int numero)
        {
            var response = _visitaApplicationService.ObterPorId(usina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("v1/visita/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarAnexo([FromBody] VisitaAnexoAdicionarRequest anexo)
        {
            var id = _visitaApplicationService.AdicionarAnexo(User.Identity.Name, anexo);

            return CreateResponse(HttpStatusCode.OK, id);
        }

        [HttpGet]
        [Route("v1/visita/anexo/usina/{usina}/ano/{anoVisita}/numero/{numeroVisita}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAnexos(int usina, int anoVisita, int numeroVisita)
        {
            var anexos = _visitaApplicationService.ListarAnexos(usina, anoVisita, numeroVisita);

            return CreateResponse(HttpStatusCode.OK, anexos);
        }

        [HttpGet]
        [Route("v1/visita/anexo/{id}")]
        [Authorize]
        public string ObterAnexo(string id)
        {
            var anexo = _visitaApplicationService.ObterAnexo(Guid.Parse(id));
            var visitaAnexo = _visitaApplicationService.ObterVisitaAnexoPorId(Guid.Parse(id));

            return _intervenienteApplicationService.ObterAnexoConvertidoBase64(anexo, (visitaAnexo != null ? visitaAnexo.Nome : ""));
        }

        [HttpPatch]
        [Route("v1/visita/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarDescricaoAnexo([FromBody] VisitaAnexoAtualizarRequest anexo)
        {
            _visitaApplicationService.AtualizarDescricaoAnexo(anexo);

            return CreateResponse(HttpStatusCode.OK, "Descrição atualizada com sucesso!");
        }

        [HttpDelete]
        [Route("v1/visita/anexo/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> RemoverAnexo(string id)
        {
            _visitaApplicationService.RemoverAnexo(Guid.Parse(id));

            return CreateResponse(HttpStatusCode.OK, "Anexo deletado com Sucesso");
        }

        [HttpPost]
        [Route("v1/visita/historico")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarHistorico([FromBody] VisitaHistoricoAdicionarRequest request)
        {

            _visitaApplicationService.AdicionarHistorico(request, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Histórico adicionado com Sucesso");

        }

        [HttpGet]
        [Route("v1/visita/historico")]
        [Authorize]
        public Task<HttpResponseMessage> ListarHistoricoEmOrdemDecrescente([FromUri] VisitaHistoricoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<VisitaHistorico>(request.Filter);
            var pagedList = _visitaApplicationService.ListarHistoricoEmOrdemDecrescente(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }
    }
}