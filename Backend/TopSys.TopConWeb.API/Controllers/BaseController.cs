using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;
using Topsys.TopConWeb.SharedKernel.Common;
using System.IO;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;

namespace TopSys.TopConWeb.API.Controllers
{
    [ValidationModel]
    public class BaseController : ApiController
    {
        public DomainNotificationHandler Notifications;

        //Padrão de retorno de mensagem e troca de mensagens HTTP
        public HttpResponseMessage Responsemessage;

        public BaseController()
        {
            Notifications = DomainEvent.Container.GetService<DomainNotificationHandler>();
            this.Responsemessage = new HttpResponseMessage();
        }

        //TODO: Refatorar para trabalhar com domain notifications
        public Task<HttpResponseMessage> CreateResponse(HttpStatusCode code, object result)
        {
            if (!Notifications.HasNotifications())
                Responsemessage = Request.CreateResponse(code, result);
            else
                Responsemessage = Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = this.Notifications.Notify()});

            Responsemessage.Headers.Add("api-date-version", VersionHelper.topconApiDateVersion);
            Responsemessage.Headers.Add("Access-Control-Expose-Headers", "api-date-version");

            return Task.FromResult<HttpResponseMessage>(Responsemessage);
        }

        public Task<HttpResponseMessage> CreateResponse(object result)
        {
            return this.CreateResponse(GetHttpStatusCode(result), result);
        }

        public Task<HttpResponseMessage> CreatePagedResponse<T>(HttpStatusCode code, PagedList<T> pagedList)
        {
            if (!Notifications.HasNotifications())
                Responsemessage = Request.CreateResponse(code, pagedList.Records);
            else
                Responsemessage = Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = this.Notifications.Notify() });
            
            Responsemessage.Headers.Add("page-info", pagedList.Info);
            Responsemessage.Headers.Add("Access-Control-Expose-Headers", "page-info");
            Responsemessage.Headers.Add("api-date-version", VersionHelper.topconApiDateVersion);
            Responsemessage.Headers.Add("Access-Control-Expose-Headers", "api-date-version");

            return Task.FromResult<HttpResponseMessage>(Responsemessage);
        }
        public HttpResponseMessage CreatePdfResponse(HttpStatusCode code, Stream result)
        {
            if (Notifications.HasNotifications())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = this.Notifications.Notify() });
            }

            var response = new HttpResponseMessage(code)
            {
                Content = new ByteArrayContent(ReadFully(result))
            };
            response.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"); //octet-stream

            response.Headers.Add("api-date-version", VersionHelper.topconApiDateVersion);
            response.Headers.Add("Access-Control-Expose-Headers", "api-date-version");

            return response;
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static string StreamToString(Stream stream)
        {
            var stringStream = new StreamReader(stream);
            stringStream.BaseStream.Seek(0, SeekOrigin.Begin);
            return stringStream.ReadToEnd();
        }

        protected override void Dispose(bool disposing)
        {
            Notifications.Dispose();
        }

        public static HttpStatusCode GetHttpStatusCode(object result)
        {
            if (result is null)
                return HttpStatusCode.InternalServerError;

            var statusProperty = result.GetType().GetProperty("Status");

            if (statusProperty != null)
            {
                var statusValue = statusProperty.GetValue(result);

                if (statusValue.ToString() == "success")
                    return HttpStatusCode.OK;

                var errorCodeProperty = result.GetType().GetProperty("ErrorCode");
                var errorCodeValue = errorCodeProperty is null ? "" : errorCodeProperty.GetValue(result);

                if (errorCodeValue.ToString() == "500")
                    return HttpStatusCode.InternalServerError;

                if (errorCodeValue.ToString() == "400")
                    return HttpStatusCode.BadRequest;

                return HttpStatusCode.PreconditionFailed;
            }

            return HttpStatusCode.InternalServerError;
        }

    }
}
