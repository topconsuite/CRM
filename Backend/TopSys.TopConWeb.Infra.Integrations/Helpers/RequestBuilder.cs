using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.Helpers
{
    public static class RequestBuilder
    {
        public static RestRequest Build(Method method, string resource)
        {

            var request = new RestRequest()
            {
                RequestFormat = DataFormat.Json,
                Method = method,
                Resource = resource
            };

            request.OnBeforeDeserialization = (x =>
            {
                x.ContentType = "application/json; charset=utf-8";

                if (x.Content.Length == 0)
                    x.Content = x.Content.Insert(0, "{}");
            });

            return request;
        }
    }
}
