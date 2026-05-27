using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Adicionar;
using TopSys.TopConWeb.Application.DTOS.Request.Opportunity.Alteracao;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class OpportunityController : BaseController
    {
        private readonly IOpportunityApplicationService _opportunityApplicationService;

        public OpportunityController(IOpportunityApplicationService opportunityApplicationService)
        {
            _opportunityApplicationService = opportunityApplicationService;
        }

        [HttpGet]
        [Route("v1/opportunity/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorID(string id)
        {
            var opportunity = _opportunityApplicationService.ObterPorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunity);
        }

        [HttpGet]
        [Route("v1/opportunity")]
        [Authorize]
        public Task<HttpResponseMessage> Listar()
        {
            var opportunities = _opportunityApplicationService.Listar();
            return CreateResponse(HttpStatusCode.OK, opportunities);
        }

        [HttpPost]
        [Route("v1/opportunity/adicionar")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] OpportunityAdicionarRequest opportunity)
        {
            var newOpportunity = _opportunityApplicationService.Adicionar(opportunity);

            return CreateResponse(HttpStatusCode.OK, newOpportunity);
        }

        [HttpPatch]
        [Route("v1/opportunity/atualizar")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] OpportunityAlteracaoRequest opportunity)
        {
            _opportunityApplicationService.Atualizar(opportunity);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(string id)
        {
            _opportunityApplicationService.Deletar(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }


        /* --------------------------- OpportunityTypes --------------------------- */

        [HttpGet]
        [Route("v1/opportunity-type/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterOpportunityTypePorID(string id)
        {
            var opportunityType = _opportunityApplicationService.ObterOpportunityTypePorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunityType);
        }

        [HttpGet]
        [Route("v1/opportunity-type")]
        [Authorize]
        public Task<HttpResponseMessage> ListarOpportunityTypes()
        {
            var opportunityTypes = _opportunityApplicationService.ListarOpportunityTypes();
            return CreateResponse(HttpStatusCode.OK, opportunityTypes);
        }

        [HttpPost]
        [Route("v1/opportunity-type/adicionar")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarOpportunityType([FromBody] OpportunityTypeAdicionarRequest opportunityType)
        {
            var newOpportunityType = _opportunityApplicationService.AdicionarOpportunityType(opportunityType);
            return CreateResponse(HttpStatusCode.OK, newOpportunityType);
        }

        [HttpPatch]
        [Route("v1/opportunity-type/atualizar")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarOpportunityType([FromBody] OpportunityTypeAlteracaoRequest opportunityType)
        {
            _opportunityApplicationService.AtualizarOpportunityType(opportunityType);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity-type/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> DeletarOpportunityType(string id)
        {
            _opportunityApplicationService.DeletarOpportunityType(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }

        /* --------------------------- OpportunityFailureReasons --------------------------- */

        [HttpGet]
        [Route("v1/opportunity-failure-reason/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterOpportunityFailureReasonPorID(string id)
        {
            var opportunityFailureReason = _opportunityApplicationService.ObterOpportunityFailureReasonPorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunityFailureReason);
        }

        [HttpGet]
        [Route("v1/opportunity-failure-reason")]
        [Authorize]
        public Task<HttpResponseMessage> ListarOpportunityFailureReasons()
        {
            var opportunityFailureReasons = _opportunityApplicationService.ListarOpportunityFailureReasons();
            return CreateResponse(HttpStatusCode.OK, opportunityFailureReasons);
        }

        [HttpPost]
        [Route("v1/opportunity-failure-reason/adicionar")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarOpportunityFailureReason([FromBody] OpportunityFailureReasonAdicionarRequest opportunityFailureReason)
        {
            var newOpportunityFailureReason = _opportunityApplicationService.AdicionarOpportunityFailureReason(opportunityFailureReason);
            return CreateResponse(HttpStatusCode.OK, newOpportunityFailureReason);
        }

        [HttpPatch]
        [Route("v1/opportunity-failure-reason/atualizar")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarOpportunityFailureReason([FromBody] OpportunityFailureReasonAlteracaoRequest opportunityFailureReason)
        {
            _opportunityApplicationService.AtualizarOpportunityFailureReason(opportunityFailureReason);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity-failure-reason/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> DeletarOpportunityFailureReason(string id)
        {
            _opportunityApplicationService.DeletarOpportunityFailureReason(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }

        /* --------------------------- OpportunityOrigin --------------------------- */

        [HttpGet]
        [Route("v1/opportunity-origin/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterOpportunityOriginPorID(string id)
        {
            var opportunityOrigin = _opportunityApplicationService.ObterOpportunityOriginPorId(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, opportunityOrigin);
        }

        [HttpGet]
        [Route("v1/opportunity-origin")]
        [Authorize]
        public Task<HttpResponseMessage> ListarOpportunityOrigins()
        {
            var opportunityOrigins = _opportunityApplicationService.ListarOpportunityOrigins();
            return CreateResponse(HttpStatusCode.OK, opportunityOrigins);
        }

        [HttpPost]
        [Route("v1/opportunity-origin/adicionar")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarOpportunityOrigin([FromBody] OpportunityOriginAdicionarRequest opportunityOrigin)
        {
            var newOpportunityOrigin = _opportunityApplicationService.AdicionarOpportunityOrigin(opportunityOrigin);
            return CreateResponse(HttpStatusCode.OK, newOpportunityOrigin);
        }

        [HttpPatch]
        [Route("v1/opportunity-origin/atualizar")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarOpportunityOrigin([FromBody] OpportunityOriginAlteracaoRequest opportunityOrigin)
        {
            _opportunityApplicationService.AtualizarOpportunityOrigin(opportunityOrigin);
            return CreateResponse(HttpStatusCode.OK, "Atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("v1/opportunity-origin/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> DeletarOpportunityOrigin(string id)
        {
            _opportunityApplicationService.DeletarOpportunityOrigin(new Guid(id));
            return CreateResponse(HttpStatusCode.OK, "Deletado com sucesso.");
        }


    }
}