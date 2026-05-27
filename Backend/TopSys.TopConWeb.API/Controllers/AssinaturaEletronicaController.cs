using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Topsys.TopConWeb.SharedKernel.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.AssinaturaEletronica;
using TopSys.TopConWeb.Application.DTOS.Request.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Application.DTOS.Response.AssinaturaEletronica.Clicksign;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class AssinaturaEletronicaController : BaseController
    {
        private readonly IAssinaturaEletronicaApplicationService _assinaturaEletronicaApplicationService;
        private readonly IParametroApplicationService _parametroAppService;
        private readonly IClicksignConfiguracaoApplicationService _clicksignConfiguracaoAppService;

        public AssinaturaEletronicaController(
            IAssinaturaEletronicaApplicationService assinaturaEletronicaApplicationService,
            IParametroApplicationService parametroAppService,
            IClicksignConfiguracaoApplicationService clicksignConfiguracaoAppService)
        {
            _assinaturaEletronicaApplicationService = assinaturaEletronicaApplicationService;
            _parametroAppService = parametroAppService;
            _clicksignConfiguracaoAppService = clicksignConfiguracaoAppService;
        }

        [HttpGet]
        [Route("v1/assinatura-eletronica/clicksign")]
        [Authorize]
        public Task<HttpResponseMessage> ObterParametrosClickSign()
        {
            var parametros = _assinaturaEletronicaApplicationService.ObterParametrosClicksign();

            return CreateResponse(HttpStatusCode.OK, parametros);
        }

        [HttpGet]
        [Route("v1/assinatura-eletronica/clicksign-envios/usina/{idUsina}/ano-contrato/{ano}/numero-contrato/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarContratoEnviosClicksign(int idUsina, int ano, int numero)
        {
            var lista = _assinaturaEletronicaApplicationService.ListarContratosClicksignEnvios(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, lista);
        }

        [HttpGet]
        [Route("v1/assinatura-eletronica/clicksign-envio/usina/{idUsina}/ano-contrato/{ano}/numero-contrato/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterContratoEnviosClicksign(int idUsina, int ano, int numero)
        {
            var item = _assinaturaEletronicaApplicationService.ObterUltimoContratoClicksignEnvio(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, item);
        }

        [HttpPost]
        [Route("v1/assinatura-eletronica/clicksign")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarParametrosClickSign([FromBody] ClicksignParametrosRequest parametroRequest)
        {
            _assinaturaEletronicaApplicationService.AtualizarParametrosClicksign(parametroRequest);

            return CreateResponse(HttpStatusCode.OK, "Atualizado com Sucesso!");
        }

        [HttpPost]
        [Route("v1/assinatura-eletronica/solicitar/clicksign")]
        [Authorize]
        public Task<HttpResponseMessage> SolicitarAssinaturaClicksign([FromBody] SolicitacaoAssinaturaClicksignRequest parametroRequest)
        {
            _assinaturaEletronicaApplicationService.SolicitarAssinaturaClicksign(parametroRequest);

            return CreateResponse(HttpStatusCode.OK, "Solicitado!");
        }

        [HttpPatch]
        [Route("v1/assinatura-eletronica/cancelar/clicksign/documento/{documento}")]
        [Authorize]
        public Task<HttpResponseMessage> CancelarDocumentoClicksign(Guid documento)
        {
             _assinaturaEletronicaApplicationService.CancelarDocumentoClicksign(documento);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/assinatura-eletronica/possui-integracao")]
        [Authorize]
        public Task<HttpResponseMessage> VerificarIntegracaoConfigurada()
        {
            var possuiIntegracao = _parametroAppService.ObterParametroIntegracoes("clicksign", "access_token") != "";

            return CreateResponse(HttpStatusCode.OK, possuiIntegracao);
        }

        [HttpPost]
        [Route("v1/assinatura-eletronica/eventos")]
        public Task<HttpResponseMessage> ClicksignEventos([FromUri] int? usinaId = null)
        {
            if (!ClicksignRequestIsAuthorized(HttpContext.Current.Request, usinaId))
                return CreateResponse(HttpStatusCode.Unauthorized, "");

            var bodyText = StreamToString(HttpContext.Current.Request.InputStream);
            var clicksignEvent = JsonConvert.DeserializeObject<ClicksignEvento>(bodyText);

            _assinaturaEletronicaApplicationService.ProcessarEventoClicksign(clicksignEvent);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        /// <summary>
        /// Valida a assinatura HMAC-SHA256 do webhook ClickSign.
        /// Hierarquia de resolução do sha256-secret:
        ///   1. Se usinaId informado → busca na configuracoes_clicksign da Usina.
        ///   2. Fallback → credencial global em con_integracoes (comportamento legado).
        /// </summary>
        public bool ClicksignRequestIsAuthorized(HttpRequest request, int? usinaId = null)
        {
            var bodyText = StreamToString(request.InputStream);

            // Hierarquia de fallback do sha256-secret
            string secretKey = null;

            if (usinaId.HasValue)
                secretKey = _clicksignConfiguracaoAppService.ObterSha256SecretPorUsina(usinaId.Value);

            if (string.IsNullOrEmpty(secretKey))
                secretKey = _parametroAppService.ObterParametroIntegracoes("clicksign", "sha256-secret");

            if (request.Headers["Content-Hmac"] == null) return false;

            var headerValue = request.Headers?.GetValues("Content-Hmac").FirstOrDefault();
            headerValue = headerValue.Replace("sha256=", "");

            return headerValue == bodyText.HmacSha256Digest(secretKey);
        }

    }
}