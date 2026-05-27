using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.CadastroGeral;
using TopSys.TopConWeb.Application.DTOS.Request.Funcionario;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class FuncionarioController : BaseController
    {
        private readonly IFuncionarioApplicationService _funcionarioApplicationService;
        private readonly ICadastroGeralApplicationService _cadastroGeralApplicationService;

        public FuncionarioController(IFuncionarioApplicationService funcionarioApplicationService, ICadastroGeralApplicationService cadastroGeralApplicationService)
        {
            _funcionarioApplicationService = funcionarioApplicationService;
            _cadastroGeralApplicationService = cadastroGeralApplicationService;
        }

        [HttpGet]
        [Route("v1/funcionarios/analistas")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAnalistas()
        {
            var analistas = _funcionarioApplicationService.ListarAnalistas();

            return CreateResponse(HttpStatusCode.OK, analistas);
        }

        #region Funcionário Função

        [HttpPost]
        [Route("integrations/employee-roles")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoAdicionar([FromBody] FuncionarioFuncaoRequest request)
        {
            var result = _cadastroGeralApplicationService.Adicionar(request.FuncionariosFuncoes, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employee-roles/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoAtualizarPorId([FromUri] int id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarId(id, request, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employee-roles/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoAtualizarPorExternalId([FromUri] string external_id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarExternalId(external_id, request, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/employee-roles/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoDeletarPorId([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorId(id, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/employee-roles/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoDeletarPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorExternalId(external_id, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }



        [HttpGet]
        [Route("integrations/employee-roles")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoListar()
        {
            var result = _cadastroGeralApplicationService.Listar(ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employee-roles/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoObterPorID([FromUri]int id)
        {
            var result = _cadastroGeralApplicationService.ObterPorId(id, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employee-roles/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioFuncaoObterPorExternalId([FromUri]string external_id)
        {
            var result = _cadastroGeralApplicationService.ObterPorExternalId(external_id, ECadastroGeralTipo.FuncionarioFuncao);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        #endregion

        #region Funcionário Departamento

        [HttpPost]
        [Route("integrations/employee-departments")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoAdicionar([FromBody] FuncionarioDepartamentoRequest request)
        {
            var result = _cadastroGeralApplicationService.Adicionar(request.FuncionariosDepartamentos, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employee-departments/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoAtualizarPorId([FromUri] int id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarId(id, request, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employee-departments/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoAtualizarPorExternalId([FromUri] string external_id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarExternalId(external_id, request, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/employee-departments/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoDeletarPorId([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorId(id, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/employee-departments/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoDeletarPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorExternalId(external_id, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }



        [HttpGet]
        [Route("integrations/employee-departments")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoListar()
        {
            var result = _cadastroGeralApplicationService.Listar(ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employee-departments/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoObterPorID([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.ObterPorId(id, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employee-departments/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioDepartamentoObterPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.ObterPorExternalId(external_id, ECadastroGeralTipo.FuncionarioDepartamento);

            return CreateResponse(result);
        }

        #endregion

        #region Funcionário Status

        [HttpPost]
        [Route("integrations/employee-statuses")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusAdicionar([FromBody] FuncionarioStatusRequest request)
        {
            var result = _cadastroGeralApplicationService.Adicionar(request.FuncionariosStatus, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employee-statuses/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusAtualizarPorId([FromUri] int id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarId(id, request, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employee-statuses/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusAtualizarPorExternalId([FromUri] string external_id, [FromBody] CadastroGeralIntegracaoAtualizarRequest request)
        {
            var result = _cadastroGeralApplicationService.AtualizarExternalId(external_id, request, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/employee-statuses/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusDeletarPorId([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorId(id, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/employee-statuses/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusDeletarPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.DeletarPorExternalId(external_id, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }



        [HttpGet]
        [Route("integrations/employee-statuses")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusListar()
        {
            var result = _cadastroGeralApplicationService.Listar(ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employee-statuses/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusObterPorID([FromUri] int id)
        {
            var result = _cadastroGeralApplicationService.ObterPorId(id, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employee-statuses/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioStatusObterPorExternalId([FromUri] string external_id)
        {
            var result = _cadastroGeralApplicationService.ObterPorExternalId(external_id, ECadastroGeralTipo.FuncionarioStatus);

            return CreateResponse(result);
        }

        #endregion

        #region Funcionário

        [HttpPost]
        [Route("integrations/employees")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioAdicionar([FromBody] FuncionariosRequest request)
        {
            var result = _funcionarioApplicationService.Adicionar(request.Funcionarios);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employees/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioAtualizarPorId([FromUri] int id, [FromBody] FuncionarioAtualizarRequest request)
        {
            var result = _funcionarioApplicationService.AtualizarPorId(id, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/employees/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioAtualizarPorExternalId([FromUri] string external_id, [FromBody] FuncionarioAtualizarRequest request)
        {
            var result = _funcionarioApplicationService.AtualizarPorExternalId(external_id, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employees")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioListar()
        {
            var result = _funcionarioApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employees/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioObterPorID([FromUri] int id)
        {
            var result = _funcionarioApplicationService.ObterPorId(id);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/employees/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FuncionarioObterPorExternalId([FromUri] string external_id)
        {
            var result = _funcionarioApplicationService.ObterPorExternalId(external_id);

            return CreateResponse(result);
        }

        #endregion
    }
}