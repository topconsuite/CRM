using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Topsys.TopConWeb.SharedKernel.Resources;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;

namespace TopSys.TopConWeb.API.Helpers
{

    public class ValidationModelAttribute : ActionFilterAttribute
    {

        private const string ApiKeyHeaderName = "api_key";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var errors = new List<Error>() { };
            var headerProvider = actionContext.Request.GetDependencyScope().GetService(typeof(IHeaderProvider)) as IHeaderProvider;

            var headers = actionContext.Request.Headers;
            var preferredLanguage = headers.AcceptLanguage != null && headers.AcceptLanguage.Count > 0
            ? headers.AcceptLanguage.First().Value
            : null;

            if (!actionContext.Request.Headers.Contains(ApiKeyHeaderName)) return;

            if (!IsValidCultureInfo(preferredLanguage))
            {
                errors.Add(new Error("", "Accept-language is invalid. Supported languages: en-US, pt-BR, es"));
                var response = new ResultDTO<Error>(EResultDTOStatus.Error, "Errors occurred when validating the request", errors);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                return;
            }

            var cultureInfo = preferredLanguage is null ? null : new CultureInfo(preferredLanguage);
            headerProvider.SetAcceptLanguage(cultureInfo);


            if (!actionContext.ModelState.IsValid)
            {
                var validationErrors = actionContext.ModelState
                    .SelectMany(ms => ms.Value.Errors.Select(e => new { Position = GetPositionFromKey(ms.Key), ErrorMessage = e.ErrorMessage }))
                    .ToList();

                foreach (var validationError in validationErrors)
                {
                    var message = validationError.ErrorMessage;
                    var errorCode = "";
                    var messageParams = new List<object>();
                    if (message.Contains("::"))
                    {
                        var splitMessages = message.Split(new string[] { "::" }, StringSplitOptions.None);
                        message = splitMessages[0].Trim();
                        for (int i = 1; i < splitMessages.Length; i++)
                        {
                            messageParams.Add(splitMessages[i].Trim());
                        }
                    }

                    var rm = new ResourceManager(typeof(Resources));
                    string messageInResource = null;
                    try
                    {
                        messageInResource = rm.GetString(message, headerProvider.GetAcceptLanguage());
                        messageInResource = string.Format(messageInResource, messageParams.ToArray());
                    } catch { }

                    errors.Add(new Error(errorCode, messageInResource ?? message, validationError.Position));
                }

                var response = new ResultDTO<Error>(EResultDTOStatus.Error, "Errors occurred when validating the request body.", errors);

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                return;
            }
        }

        private static int? GetPositionFromKey(string key)
        {
            var match = Regex.Match(key, @"\[(\d+)\]");
            if (match.Success && match.Groups.Count > 1)
            {
                if (int.TryParse(match.Groups[1].Value, out int position))
                {
                    return position;
                }
            }
            return null;
        }

        private static bool IsValidCultureInfo(string stringCultureInfo)
        {
            if (stringCultureInfo is null) return true;

            var acceptedLanguages = new[] { "en-us", "pt-br", "es" };
            return acceptedLanguages.Contains(stringCultureInfo.ToLower());
        }
    }
}