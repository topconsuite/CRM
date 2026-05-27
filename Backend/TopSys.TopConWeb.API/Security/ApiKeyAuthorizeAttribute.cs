using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Security { 

    public class ApiKeyAuthorizeAttribute : AuthorizeAttribute
    {
        private const string ApiKeyHeaderName = "api_key";
        private static IParametroApplicationService ParametroApplicationService;

        public static void ConfigureApiKeyAuthorize(IParametroApplicationService parametroApplicationService)
        {
            ParametroApplicationService = parametroApplicationService;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains(ApiKeyHeaderName))
            {
                var apiKey = actionContext.Request.Headers.GetValues(ApiKeyHeaderName).FirstOrDefault();

                if (!IsBettaApiKeyValid(apiKey) && !IsLimitCreditIntegrationApiKeyValid(apiKey))
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        private bool IsBettaApiKeyValid(string apiKey)
        {

            var key = ParametroApplicationService.ObterParametroIntegracoes("betta", "apikey");

            return key.Equals(apiKey) && !key.Equals("");
        }

        private bool IsLimitCreditIntegrationApiKeyValid(string apiKey)
        {

            var key = ParametroApplicationService.ObterParametroIntegracoes("limit-credit-integration", "apikey");

            return key.Equals(apiKey) && !key.Equals("");
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "API Key inválida.");
        }
    }
}