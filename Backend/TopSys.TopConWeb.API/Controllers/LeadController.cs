using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.Lead;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class LeadController : BaseController
    {
        private readonly ILeadFaseApplicationService _faseLeadApplicationService;
        private readonly ILeadApplicationService _leadApplicationService;
        private readonly IIntervenienteApplicationService _intervenienteApplicationService;

        public LeadController(ILeadFaseApplicationService faseLeadApplicationService, ILeadApplicationService leadApplicationService, IIntervenienteApplicationService intervenienteApplicationService)
        {
            _faseLeadApplicationService = faseLeadApplicationService;
            _leadApplicationService = leadApplicationService;
            _intervenienteApplicationService = intervenienteApplicationService;
        }

        [HttpPost]
        [Route("v1/lead")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] LeadInclusaoRequest lead)
        {
            var result = _leadApplicationService.Adicionar(User.Identity.Name, lead);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPatch]
        [Route("v1/lead")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] LeadAlteracaoRequest lead)
        {
            _leadApplicationService.Atualizar(User.Identity.Name, lead);

            return CreateResponse(HttpStatusCode.OK, "Lead atualizada com sucesso!");
        }

        [HttpGet]
        [Route("v1/lead/lead-fase")]
        [Authorize]
        public Task<HttpResponseMessage> ListarFases()
        {
            IEnumerable<LeadFaseResponse> listaFaseLead = _faseLeadApplicationService.ListarTodos<LeadFaseResponse>();

            return CreateResponse(HttpStatusCode.OK, listaFaseLead);
        }

        [HttpGet]
        [Route("v1/leads")]
        [Authorize]
        public Task<HttpResponseMessage> ListarEmOrdemDecrescente([FromUri] LeadPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<Lead>(request.Filter);

            var pagedList = _leadApplicationService.ListarEmOrdemDecrescente(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/lead/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var lead = _leadApplicationService.ObterPorUsinaAnoNumero(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, lead);
        }

        [HttpGet]
        [Route("v1/lead/usina/{idUsina}/ano/{ano}/numero/{numero}/logs")]
        [Authorize]
        public Task<HttpResponseMessage> ListarLeadLogsPorId(int idUsina, int ano, int numero)
        {
            var leadLogs = _leadApplicationService.ListarLeadLogsPorId(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, leadLogs);
        }

        //Anexo
        [HttpPost]
        [Route("v1/lead/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarAnexo([FromBody] LeadAnexoAdicionarRequest anexo)
        {
            var id = _leadApplicationService.AdicionarAnexo(User.Identity.Name, anexo);

            return CreateResponse(HttpStatusCode.OK, id);
        }

        [HttpGet]
        [Route("v1/lead/anexo/usina/{usina}/ano/{anoLead}/numero/{numeroLead}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAnexos(int usina, int anoLead, int numeroLead)
        {
            var anexos = _leadApplicationService.ListarAnexos(usina, anoLead, numeroLead);

            return CreateResponse(HttpStatusCode.OK, anexos);
        }

        [HttpGet]
        [Route("v1/lead/anexo/{id}")]
        [Authorize]
        public string ObterAnexo(string id)
        {
            var anexo = _leadApplicationService.ObterAnexo(Guid.Parse(id));
            var leadAnexo = _leadApplicationService.ObterLeadAnexoPorId(Guid.Parse(id));

            return _intervenienteApplicationService.ObterAnexoConvertidoBase64(anexo, (leadAnexo != null ? leadAnexo.Nome : ""));
        }

        [HttpPatch]
        [Route("v1/lead/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarDescricaoAnexo([FromBody] LeadAnexoAtualizarRequest anexo)
        {
            _leadApplicationService.AtualizarDescricaoAnexo(anexo);

            return CreateResponse(HttpStatusCode.OK, "Descrição atualizada com sucesso!");
        }

        [HttpDelete]
        [Route("v1/lead/anexo/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> RemoverAnexo(string id)
        {
            _leadApplicationService.RemoverAnexo(Guid.Parse(id));

            return CreateResponse(HttpStatusCode.OK, "Anexo deletado com Sucesso");
        }

        //Interações
        [HttpGet]
        [Route("v1/lead/interacoes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarInteracoes([FromUri] LeadInteracaoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<LeadInteracao>(request.Filter);

            var interacoes = _leadApplicationService.ListarInteracoes(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, interacoes);
        }

        [HttpPost]
        [Route("v1/lead/interacao")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarInteracao([FromBody] LeadInteracaoAdicionarRequest interacao)
        {
             _leadApplicationService.AdicionarInteracao(User.Identity.Name, interacao);

            return CreateResponse(HttpStatusCode.OK, "Interação adicionada com Sucesso");
        }

        [HttpGet]
        [Route("v1/lead/usina/{usina}/ano/{ano}/numero/{numero}/gerar-oportunidade")]
        [Authorize]
        public Task<HttpResponseMessage> ObterOportunidadeDeLead(int usina, int ano, int numero)
        {
            var oportunidade = _leadApplicationService.ObterOportunidadeDeLead(usina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, oportunidade);
        }
    }
}