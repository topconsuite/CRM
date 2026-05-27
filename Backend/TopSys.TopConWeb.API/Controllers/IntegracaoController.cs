using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Adicionar;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente;
using TopSys.TopConWeb.Application.DTOS.Response.Vendedor;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.Integracao;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api/integracao")]
    [ApiKeyAuthorize]
    public class IntegracaoController : BaseController
    {

        private readonly IIntervenienteApplicationService _intervenienteAppService;
        private readonly IVendedorApplicationService _vendedorApplicationService;
        private readonly IPropostaApplicationService _propostaAppService;
        private readonly IEnderecoApplicationService _enderecoAppService;
        private readonly IIntegracaoApplicationService _integracaoApplicationService;
        private readonly IOpportunityApplicationService _opportunityApplicationService;
        private readonly IUsinaApplicationService _usinaApplicationService;

        public IntegracaoController(
            IIntervenienteApplicationService intervenienteAppService,
            IVendedorApplicationService vendedorApplicationService,
            IPropostaApplicationService propostaAppService,
            IEnderecoApplicationService enderecoAppService,
            IIntegracaoApplicationService integracaoApplicationService,
            IOpportunityApplicationService opportunityApplicationService,
            IUsinaApplicationService usinaApplicationService
            )
        {
            _intervenienteAppService = intervenienteAppService;
            _vendedorApplicationService = vendedorApplicationService;
            _propostaAppService = propostaAppService;
            _enderecoAppService = enderecoAppService;
            _integracaoApplicationService = integracaoApplicationService;
            _opportunityApplicationService = opportunityApplicationService;
            _usinaApplicationService = usinaApplicationService;
        }

        [HttpGet]
        [Route("v1/usinas")]
        public Task<HttpResponseMessage> UsinaListarTodos()
        {
            var listaUsinas = _usinaApplicationService.ListarUsinasAtivas();

            return CreateResponse(HttpStatusCode.OK, listaUsinas);
        }

        [HttpGet]
        [Route("v1/interveniente/codigo/{intervenienteCodigo}")]
        public Task<HttpResponseMessage> IntervenienteObterPorCodigo(int intervenienteCodigo)
        {
            var filterLambda = UrlFilterParser.Parse<Interveniente>($"$(Codigo=={intervenienteCodigo})");

            var interveniente = _intervenienteAppService
            .ListarFiltrados<IntervenienteResponse>(filterLambda, t => t.EnderecoMunicipio, t => t.Vendedor, t => t.BloqueioMotivo)
            .FirstOrDefault();


            return CreateResponse(HttpStatusCode.OK, interveniente);
        }

        [HttpPost]
        [Route("v1/interveniente")]
        public Task<HttpResponseMessage> Adicionar(IntervenienteInclusaoRequest interveniente)
        {
            var result = _intervenienteAppService.Adicionar(interveniente, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPatch]
        [Route("v1/interveniente")]    
        public Task<HttpResponseMessage> Atualizar(IntervenienteAlteracaoRequest interveniente)
        {
            _intervenienteAppService.Atualizar(interveniente, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Interveniente atualizado com sucesso.");
        }

        [HttpGet]
        [Route("v1/interveniente/cpfCnpj/{cpfCnpj}/inscricao-estadual/{inscricaoEstadual}")]
        public Task<HttpResponseMessage> IntervenienteObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual)
        {
            var interveniente = _intervenienteAppService.ObterPorCpfCnpj(cpfCnpj, inscricaoEstadual);

            return CreateResponse(HttpStatusCode.OK, interveniente);
        }

        [HttpGet]
        [Route("v1/municipios")]
        public Task<HttpResponseMessage> MunicipioListarMunicipios()
        {
            var municipios = _enderecoAppService.ListarMunicipios();

            return CreateResponse(HttpStatusCode.OK, municipios);
        }

        [HttpGet]
        [Route("v1/vendedores")]
        public Task<HttpResponseMessage> VendedoresListarPermitidos()
        {

            var listaVendedores = _vendedorApplicationService.ListarFiltrados<VendedorResponse>((t => t.Ativo == "S"));

            return CreateResponse(HttpStatusCode.OK, listaVendedores);
        }

        [HttpGet]
        [Route("v1/propostas/cpf-cnpj/{cpfCnpj}/pagina/{pagina}/por-pagina/{porPagina}")]
        public Task<HttpResponseMessage> PropostaPorCpfCnpj(string cpfCnpj, int pagina, int porPagina)
        {
            var pagedList = _propostaAppService.ListarPorCpfCnpj(cpfCnpj, pagina, porPagina);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }
        
        [HttpGet]
        [Route("v1/proposta/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        public Task<HttpResponseMessage> PropostaObterPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var proposta = _propostaAppService.ObterPorUsinaAnoNumero(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, proposta);
        }

        [HttpPatch]
        [Route("v1/remessa/horarios")]
        public Task<HttpResponseMessage> SalvarRetornoHorariosBetoneiraLab360(IntegracaoRetornoHorariosBetoneira retorno)
        {

            _integracaoApplicationService.SalvarRetornoHorariosBetoneiraLab360(retorno);

            return CreateResponse(HttpStatusCode.OK, "Horários da remessa atualizados com sucesso.");

        }

        [HttpGet]
        [Route("v1/remessas/totais-por-contrato-e-cliente/{codCliente}")]
        public Task<HttpResponseMessage> ObterTotaisRemessaContratosPorCliente(int codCliente)
        {
            var totais = _integracaoApplicationService.ObterTotaisRemessaContratosPorCliente(codCliente);

            return CreateResponse(HttpStatusCode.OK, totais);
        }

        [HttpGet]
        [Route("v1/interveniente/totais-contas-a-receber-e-remessa/cpfCnpj/{cpfCnpj}/inscricao-estadual/{inscricaoEstadual}")]
        public Task<HttpResponseMessage> ObterTotaisContasReceberPorCliente(string cpfCnpj, string inscricaoEstadual)
        {
            var totais = _integracaoApplicationService.ObterTotaisContasReceberPorCliente(cpfCnpj, inscricaoEstadual);

            return CreateResponse(HttpStatusCode.OK, totais);
        }


        /* -- Opportunidades  --*/

        [HttpGet]
        [Route("v1/opportunity/{id}")]
        public Task<HttpResponseMessage> ObterPorID(string id)
        {
            var opportunity = _opportunityApplicationService.ObterPorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunity);
        }

        [HttpGet]
        [Route("v1/opportunity")]
        public Task<HttpResponseMessage> OportunidadeListar([FromUri] OpportunityPagedRequest request)
        { 
            var urlFilter = UrlFilterParser.Parse<Opportunity>(request.Filter);
        
            var opportunities = _opportunityApplicationService.ListarPorPagina(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, opportunities);
        }

        [HttpPost]
        [Route("v1/opportunity")]
        public Task<HttpResponseMessage> OportunidadeAdicionar([FromBody] OpportunityAdicionarRequest opportunity)
        {
            var newOpportunity = _opportunityApplicationService.Adicionar(opportunity);

            return CreateResponse(HttpStatusCode.OK, newOpportunity);
        }

        [HttpPatch]
        [Route("v1/opportunity")]
        public Task<HttpResponseMessage> OportunidadeAtualizar([FromBody] OpportunityAlteracaoRequest opportunity)
        {
            _opportunityApplicationService.Atualizar(opportunity);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity/{id}")]
        public Task<HttpResponseMessage> OportunidadeDeletar(string id)
        {
            _opportunityApplicationService.Deletar(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }


        /* --------------------------- OpportunityTypes --------------------------- */

        [HttpGet]
        [Route("v1/opportunity-type/{id}")]
        public Task<HttpResponseMessage> ObterOpportunityTypePorID(string id)
        {
            var opportunityType = _opportunityApplicationService.ObterOpportunityTypePorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunityType);
        }

        [HttpGet]
        [Route("v1/opportunity-type")]
        public Task<HttpResponseMessage> ListarOpportunityTypes()
        {
            var opportunityTypes = _opportunityApplicationService.ListarOpportunityTypes();
            return CreateResponse(HttpStatusCode.OK, opportunityTypes);
        }

        [HttpPost]
        [Route("v1/opportunity-type")]
        public Task<HttpResponseMessage> AdicionarOpportunityType([FromBody] OpportunityTypeAdicionarRequest opportunityType)
        {
            var newOpportunityType = _opportunityApplicationService.AdicionarOpportunityType(opportunityType);
            return CreateResponse(HttpStatusCode.OK, newOpportunityType);
        }

        [HttpPatch]
        [Route("v1/opportunity-type")]
        public Task<HttpResponseMessage> AtualizarOpportunityType([FromBody] OpportunityTypeAlteracaoRequest opportunityType)
        {
            _opportunityApplicationService.AtualizarOpportunityType(opportunityType);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity-type/{id}")]
        public Task<HttpResponseMessage> DeletarOpportunityType(string id)
        {
            _opportunityApplicationService.DeletarOpportunityType(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }

        /* --------------------------- OpportunityFailureReasons --------------------------- */

        [HttpGet]
        [Route("v1/opportunity-failure-reason/{id}")]
        public Task<HttpResponseMessage> ObterOpportunityFailureReasonPorID(string id)
        {
            var opportunityFailureReason = _opportunityApplicationService.ObterOpportunityFailureReasonPorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunityFailureReason);
        }

        [HttpGet]
        [Route("v1/opportunity-failure-reason")]
        public Task<HttpResponseMessage> ListarOpportunityFailureReasons()
        {
            var opportunityFailureReasons = _opportunityApplicationService.ListarOpportunityFailureReasons();
            return CreateResponse(HttpStatusCode.OK, opportunityFailureReasons);
        }

        [HttpPost]
        [Route("v1/opportunity-failure-reason")]
        public Task<HttpResponseMessage> AdicionarOpportunityFailureReason([FromBody] OpportunityFailureReasonAdicionarRequest opportunityFailureReason)
        {
            var newOpportunityFailureReason = _opportunityApplicationService.AdicionarOpportunityFailureReason(opportunityFailureReason);
            return CreateResponse(HttpStatusCode.OK, newOpportunityFailureReason);
        }

        [HttpPatch]
        [Route("v1/opportunity-failure-reason")]
        public Task<HttpResponseMessage> AtualizarOpportunityFailureReason([FromBody] OpportunityFailureReasonAlteracaoRequest opportunityFailureReason)
        {
            _opportunityApplicationService.AtualizarOpportunityFailureReason(opportunityFailureReason);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity-failure-reason/{id}")]
        public Task<HttpResponseMessage> DeletarOpportunityFailureReason(string id)
        {
            _opportunityApplicationService.DeletarOpportunityFailureReason(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }

        /* --------------------------- OpportunityOrigin --------------------------- */

        [HttpGet]
        [Route("v1/opportunity-origin/{id}")]
        public Task<HttpResponseMessage> ObterOpportunityOriginPorID(string id)
        {
            var opportunityOrigin = _opportunityApplicationService.ObterOpportunityOriginPorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunityOrigin);
        }

        [HttpGet]
        [Route("v1/opportunity-origin")]
        public Task<HttpResponseMessage> ListarOpportunityOrigins()
        {
            var opportunityOrigins = _opportunityApplicationService.ListarOpportunityOrigins();
            return CreateResponse(HttpStatusCode.OK, opportunityOrigins);
        }

        [HttpPost]
        [Route("v1/opportunity-origin")]
        public Task<HttpResponseMessage> AdicionarOpportunityOrigin([FromBody] OpportunityOriginAdicionarRequest opportunityOrigin)
        {
            var newOpportunityOrigin = _opportunityApplicationService.AdicionarOpportunityOrigin(opportunityOrigin);
            return CreateResponse(HttpStatusCode.OK, newOpportunityOrigin);
        }

        [HttpPatch]
        [Route("v1/opportunity-origin")]
        public Task<HttpResponseMessage> AtualizarOpportunityOrigin([FromBody] OpportunityOriginAlteracaoRequest opportunityOrigin)
        {
            _opportunityApplicationService.AtualizarOpportunityOrigin(opportunityOrigin);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity-origin/{id}")]
        public Task<HttpResponseMessage> DeletarOpportunityOrigin(string id)
        {
            _opportunityApplicationService.DeletarOpportunityOrigin(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }


    }
}