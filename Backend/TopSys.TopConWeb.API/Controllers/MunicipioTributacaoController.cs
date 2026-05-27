using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao;
using TopSys.TopConWeb.API.Security;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class MunicipioTributacaoController : BaseController
    {
        private readonly IMunicipioTributacaoApplicationService _municipioTributacaoApplicationService;

        public MunicipioTributacaoController(IMunicipioTributacaoApplicationService municipioTributacaoApplicationService)
        {
            _municipioTributacaoApplicationService = municipioTributacaoApplicationService;
        }

        [HttpPost]
        [Route("integrations/municipal-taxes")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoAdicionar([FromBody] MunicipioTributacaoRequest request)
        {
            var result = _municipioTributacaoApplicationService.Adicionar(request.MunicipiosTributacao);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/municipal-taxes/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoAtualizarPorId(int code, [FromBody] MunicipioTributacaoAtualizarRequest request)
        {
            var result = _municipioTributacaoApplicationService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/municipal-taxes/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoAtualizarPorExternalId([FromUri(Name = "external_id")] string externalId, [FromBody] MunicipioTributacaoAtualizarRequest request)
        {
            var result = _municipioTributacaoApplicationService.AtualizarPorExternalId(externalId, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/municipal-taxes/by-ibge-code/{ibge_code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoAtualizarPorIbgeCode([FromUri(Name = "ibge_code")] int ibgeCode, [FromBody] MunicipioTributacaoAtualizarRequest request)
        {
            var result = _municipioTributacaoApplicationService.AtualizarPorIbgeCode(ibgeCode, request);

            return CreateResponse(result);
        }

        [HttpGet]
		[Route("integrations/municipal-taxes")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoListar([FromUri(Name = "uf-acronym")] string ufAcronym = null)
		{
			var result = _municipioTributacaoApplicationService.Listar(ufAcronym);

            return CreateResponse(result);
		}

		[HttpGet]
		[Route("integrations/municipal-taxes/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoObterPorID(int code)
		{
			var result = _municipioTributacaoApplicationService.ObterPorId(code);

            return CreateResponse(result);
		}

		[HttpGet]
		[Route("integrations/municipal-taxes/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoObterPorExternalId([FromUri(Name = "external_id")] string externalId)
		{
			var result = _municipioTributacaoApplicationService.ObterPorExternalId(externalId);

			return CreateResponse(result);
		}

        [HttpGet]
        [Route("integrations/municipal-taxes/by-ibge-code/{ibge_code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> MunicipioTributacaoObterPorIbgeCode([FromUri(Name = "ibge_code")] int ibgeCode)
        {
            var result = _municipioTributacaoApplicationService.ObterPorIbgeCode(ibgeCode);

            return CreateResponse(result);
        }
    }

}