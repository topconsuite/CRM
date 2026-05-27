using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Request.Equipamento;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class EquipamentoController : BaseController
    {

        private readonly ICadastroGeralApplicationService _cadastroGeralApplicationService;
        private readonly IEquipamentoApplicationService _equipamentoApplicationService;

        public EquipamentoController(ICadastroGeralApplicationService cadastroGeralApplicationService, IEquipamentoApplicationService equipamentoApplicationService)
        {
            _cadastroGeralApplicationService = cadastroGeralApplicationService;
            _equipamentoApplicationService = equipamentoApplicationService;
        }

        #region Equipamento tipo

        [HttpPost]
        [Route("integrations/equipment-types")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoAdicionar([FromBody] EquipamentoTipoRequest request)
        {
            var result = _cadastroGeralApplicationService.Adicionar(request.EquipamentoTipos, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/equipment-types/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoAtualizarPorId([FromUri] int id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarId(id, request, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/equipment-types/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoAtualizarPorExternalId([FromUri] string external_id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarExternalId(external_id, request, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/equipment-types/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoDeletarPorId([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorId(id, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/equipment-types/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoDeletarPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorExternalId(external_id, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }



        [HttpGet]
        [Route("integrations/equipment-types")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoListar()
        {
            var result = _cadastroGeralApplicationService.Listar(ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/equipment-types/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoObterPorID([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.ObterPorId(id, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/equipment-types/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> EquipamentoTipoObterPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.ObterPorExternalId(external_id, ECadastroGeralTipo.EquipamentoTipo);

            return CreateResponse(result);
        }

        #endregion

        #region Integração Equipamento

        [HttpPost]
        [Route("integrations/equipments")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoAdicionar([FromBody] EquipamentosRequest request) {

            return CreateResponse(_equipamentoApplicationService.Adicionar(request.Equipamentos));
        }

        [HttpGet]
        [Route("integrations/equipments/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoObterPorID([FromUri]string code)
        {

            return CreateResponse(_equipamentoApplicationService.ObterPorID(code));

        }

        [HttpGet]
        [Route("integrations/equipments/by-external-id/{externalId}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoObterPorExternalID([FromUri] string externalId)
        {

            return CreateResponse(_equipamentoApplicationService.ObterPorExternalID(externalId));

        }

        [HttpPatch]
        [Route("integrations/equipments/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoAtualizarPorID([FromUri] string code, [FromBody] EquipamentoAtualizarRequest request)
        {

            return CreateResponse(_equipamentoApplicationService.AtualizarPorID(code, request));

        }

        [HttpPatch]
        [Route("integrations/equipments/by-external-id/{externalId}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoAtualizarPorExternalID([FromUri] string externalId, [FromBody] EquipamentoAtualizarRequest request)
        {

            return CreateResponse(_equipamentoApplicationService.AtualizarPorExternalID(externalId, request));

        }

        [HttpDelete]
        [Route("integrations/equipments/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoDeletarPorID([FromUri] string code)
        {

            return CreateResponse(_equipamentoApplicationService.DeletarPorID(code));

        }

        [HttpDelete]
        [Route("integrations/equipments/by-external-id/{externalId}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntegracaoDeletarPorExternalID([FromUri] string externalId)
        {

            return CreateResponse(_equipamentoApplicationService.DeletarPorExternalID(externalId));

        }

        #endregion

    }
}