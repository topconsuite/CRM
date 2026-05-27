using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceber;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.SharedKernel;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TituloContasAReceberController : BaseController
    {
        private readonly ITituloContasAReceberApplicationService _tituloContasAReceberAppService;
        private readonly IHeaderProvider _headerProvider;

        public TituloContasAReceberController(ITituloContasAReceberApplicationService tituloContasAReceberAppService, IHeaderProvider headerProvider)
        {
            _tituloContasAReceberAppService = tituloContasAReceberAppService;
            _headerProvider = headerProvider;
        }

        [HttpGet]
        [Route("v1/contas-a-receber/numero-cartao/{numeroCartao}/autorizacao/{autorizacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao)
        {
            var result = _tituloContasAReceberAppService.ListarPorNumeroCartaoAutorizacao(numeroCartao, autorizacao);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/contas-a-receber/usina/{idUsina}/contrao-ano/{contratoAno}/contrao-numero/{contratoNumero}/numero-cartao/{numeroCartao}/autorizacao/{autorizacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao)
        {
            var result = _tituloContasAReceberAppService.ListarPorNumeroCartaoAutorizacaoDuplicado(idUsina, contratoAno, contratoNumero,  numeroCartao, autorizacao);

            return CreateResponse(HttpStatusCode.OK, result);
        }


        //FOR PUBLIC INTEGRATIONS

        [HttpGet]
        [Route(
            "integrations/accounts-receivable/{company}/{document-type}/{document-serie}/{document-number}/{sequence}/{bank-brand-code}/{agency-number}/{account-number}/{account-verification-digit}/{unfolding}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberObterEspecifico([FromUri(Name = "company")] int empresa,
            [FromUri(Name = "document-type")] int tipoDocumento,
            [FromUri(Name = "document-serie")] string serieDocumento,
            [FromUri(Name = "document-number")] int numeroDocumento, [FromUri(Name = "sequence")] int sequencia,
            [FromUri(Name = "bank-brand-code")] int codBancoBand, [FromUri(Name = "agency-number")] int numAgencia,
            [FromUri(Name = "account-number")] int numConta,
            [FromUri(Name = "account-verification-digit")] int numContaDv,
            [FromUri(Name = "unfolding")] int desdobramento)
        {
            var result = _tituloContasAReceberAppService.ObterPorParametros(empresa, tipoDocumento, serieDocumento,
                numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento);

            return CreateResponse(result);
        }
        
        [HttpGet]
        [Route(
            "integrations/accounts-receivable/{company}/{document-type}/{document-serie}/{document-number}/{bank-brand-code}/{agency-number}/{account-number}/{account-verification-digit}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberObterEspecifico([FromUri(Name = "company")] int empresa,
            [FromUri(Name = "document-type")] int tipoDocumento,
            [FromUri(Name = "document-serie")] string serieDocumento,
            [FromUri(Name = "document-number")] int numeroDocumento,
            [FromUri(Name = "bank-brand-code")] int codBancoBand, [FromUri(Name = "agency-number")] int numAgencia,
            [FromUri(Name = "account-number")] int numConta,
            [FromUri(Name = "account-verification-digit")] int numContaDv)
        {
            var result = _tituloContasAReceberAppService.ObterPorParametros(empresa, tipoDocumento, serieDocumento,
                numeroDocumento, codBancoBand, numAgencia, numConta, numContaDv);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/accounts-receivable/third-parties/regua-de-cobranca/{original_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberObterIdReguaDeCobranca([FromUri(Name = "original_id")] string id)
        {
            var result = _tituloContasAReceberAppService.ObterPorIdReguaDeCobranca(id);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route(
            "integrations/accounts-receivable/{company}/{document-type}/{document-number}/{sequence}/{bank-brand-code}/{agency-number}/{account-number}/{account-verification-digit}/{unfolding}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberObterEspecifico([FromUri(Name = "company")] int empresa,
            [FromUri(Name = "document-type")] int tipoDocumento,
            [FromUri(Name = "document-number")] int numeroDocumento, [FromUri(Name = "sequence")] int sequencia,
            [FromUri(Name = "bank-brand-code")] int codBancoBand, [FromUri(Name = "agency-number")] int numAgencia,
            [FromUri(Name = "account-number")] int numConta,
            [FromUri(Name = "account-verification-digit")] int numContaDv,
            [FromUri(Name = "unfolding")] int desdobramento)
        {
            var result = _tituloContasAReceberAppService.ObterPorParametros(empresa, tipoDocumento, "", numeroDocumento,
                sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento);

            return CreateResponse(result);
        }
        
        [HttpGet]
        [Route(
            "integrations/accounts-receivable/{company}/{document-type}/{document-number}/{bank-brand-code}/{agency-number}/{account-number}/{account-verification-digit}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberObterEspecifico([FromUri(Name = "company")] int empresa,
            [FromUri(Name = "document-type")] int tipoDocumento,
            [FromUri(Name = "document-number")] int numeroDocumento,
            [FromUri(Name = "bank-brand-code")] int codBancoBand, [FromUri(Name = "agency-number")] int numAgencia,
            [FromUri(Name = "account-number")] int numConta,
            [FromUri(Name = "account-verification-digit")] int numContaDv)
        {
            var result = _tituloContasAReceberAppService.ObterPorParametros(empresa, tipoDocumento, "", numeroDocumento,
                codBancoBand, numAgencia, numConta, numContaDv);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/accounts-receivable")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberListar()
        {
            var queryParameters = HttpContext.Current.Request.QueryString;

            DateTime? dataEmissao = null;
            DateTime? dataOperacao = null;

            if (queryParameters["emission-date"] != null)
            {
                try
                {
                    dataEmissao = DateTime.ParseExact(queryParameters["emission-date"], "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                    if (dataEmissao != null && dataEmissao.Value.TimeOfDay != TimeSpan.Zero)
                        dataEmissao = DateTime.SpecifyKind(dataEmissao ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
                }
                catch (FormatException ex) { 
                    return CreateResponse(
                        HttpStatusCode.OK,
                        new ResultDTO<PublicoTituloContasAReceberResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "Emission date queried"),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode()));
                }
            }

            if (queryParameters["operation-date"] != null)
            {
                try
                {
                    dataOperacao = DateTime.ParseExact(queryParameters["operation-date"], "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                    if (dataOperacao != null && dataOperacao.Value.TimeOfDay != TimeSpan.Zero)
                        dataOperacao = DateTime.SpecifyKind(dataOperacao ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
                }
                catch (FormatException ex) { 
                    return CreateResponse(
                        HttpStatusCode.OK,
                        new ResultDTO<PublicoTituloContasAReceberResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "Operation date queried"),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode()));
                }
            }

            int.TryParse(queryParameters["document-type"], out int tipoDocumento);

            string serieDocumento = queryParameters.GetQueryStringValue("document-serie");

            int? centroCusto = null;
            int? numeroDocumento = null;

            if (int.TryParse(queryParameters["cost-center"], out var tmpCentro))
                centroCusto = tmpCentro;

            if (int.TryParse(queryParameters["document-number"], out var tmpNumero))
                numeroDocumento = tmpNumero;

            int.TryParse(queryParameters["client"], out int cliente);

            int.TryParse(queryParameters["page"], out int pagina);
            int.TryParse(queryParameters["limit"], out int limite);

            var result = _tituloContasAReceberAppService.Listar(
                dataEmissao, dataOperacao, tipoDocumento, centroCusto, serieDocumento, numeroDocumento, cliente, pagina, limite);

            return CreateResponse(result);
        }

        [HttpPost]
        [Route("integrations/accounts-receivable")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberAdicionar([FromBody] [Required] TituloContasAReceberRequest request)
        {
            var result = _tituloContasAReceberAppService.TituloContasAReceberAdicionar(request.TitulosContasAReceber); 

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route(
            "integrations/accounts-receivable/{company}/{document-type}/{document-serie}/{document-number}/{sequence}/{bank-brand-code}/{agency-number}/{account-number}/{account-verification-digit}/{unfolding}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberAtualizar([FromUri(Name = "company")] int empresa,
            [FromUri(Name = "document-type")] int tipoDocumento,
            [FromUri(Name = "document-serie")] string serieDocumento,
            [FromUri(Name = "document-number")] int numeroDocumento, [FromUri(Name = "sequence")] int sequencia,
            [FromUri(Name = "bank-brand-code")] int codBancoBand, [FromUri(Name = "agency-number")] int numAgencia,
            [FromUri(Name = "account-number")] int numConta,
            [FromUri(Name = "account-verification-digit")] int numContaDv,
            [FromUri(Name = "unfolding")] int desdobramento,
            [FromBody] [Required] TituloContasAReceberAtualizarRequest request)
        {
            var result = _tituloContasAReceberAppService.TituloContasAReceberAtualizar(empresa, tipoDocumento,
                serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv,
                desdobramento, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route(
            "integrations/accounts-receivable/{company}/{document-type}/{document-number}/{sequence}/{bank-brand-code}/{agency-number}/{account-number}/{account-verification-digit}/{unfolding}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> TituloContasAReceberAtualizar([FromUri(Name = "company")] int empresa,
            [FromUri(Name = "document-type")] int tipoDocumento,
            [FromUri(Name = "document-number")] int numeroDocumento, [FromUri(Name = "sequence")] int sequencia,
            [FromUri(Name = "bank-brand-code")] int codBancoBand, [FromUri(Name = "agency-number")] int numAgencia,
            [FromUri(Name = "account-number")] int numConta,
            [FromUri(Name = "account-verification-digit")] int numContaDv,
            [FromUri(Name = "unfolding")] int desdobramento,
            [FromBody] [Required] TituloContasAReceberAtualizarRequest request)
        {
            var result = _tituloContasAReceberAppService.TituloContasAReceberAtualizar(empresa, tipoDocumento, "",
                numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/accounts-receivable/by-update-date")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FaturaObterPorDataAtualizacao(
            [FromUri][Required] DateTime start_date,
            DateTime? end_date = null,
            int? page = null,
            int? limit = null)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _tituloContasAReceberAppService.ObterPorDataAtualizacao(start_date, end_date, page, limit);

            return CreateResponse(result);
        }
    }
}