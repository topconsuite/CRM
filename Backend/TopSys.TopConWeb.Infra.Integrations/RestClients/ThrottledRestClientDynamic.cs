using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Infra.Integrations.RestClients
{
    public class ThrottledRestClientDynamic : RestClient
    {

        private const int TOO_MANY_REQUESTS = 429;

        public ThrottledRestClientDynamic()
        {
        }

        public ThrottledRestClientDynamic(string url)
        {
            this.BaseUrl = new Uri(url);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public ThrottledRestClientDynamic(string url, string login, string password)
        {
            this.BaseUrl = new Uri(url);
            this.Authenticator = new HttpBasicAuthenticator(login, password);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public ThrottledRestClientDynamic(string url, string authString)
        {
            this.BaseUrl = new Uri(url);
            this.AddDefaultHeader("Authorization", authString);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public override IRestResponse Execute(IRestRequest request)
        {

            IRestResponse response;

            var wait = 0;

            do
            {
                if (wait > 0)
                    Thread.Sleep(wait);

                response = base.Execute(request);

                if (response.StatusCode.Equals((HttpStatusCode)TOO_MANY_REQUESTS))
                {
                    wait = GetMilisecondsFromAUnixTimeStamp(response.Headers.FirstOrDefault(t => t.Name.Equals("X-Rate-Limit-Reset"))?.Value);
                }
            }
            while (response.StatusCode.Equals((HttpStatusCode)TOO_MANY_REQUESTS));

            return response;

        }

        public override IRestResponse<T> Execute<T>(IRestRequest request)
        {

            IRestResponse<T> response;

            int wait = 0;

            do
            {
                if (wait > 0)
                    Thread.Sleep(wait);

                response = base.Execute<T>(request);

                if (response.StatusCode.Equals((HttpStatusCode)TOO_MANY_REQUESTS))
                {
                    wait = GetMilisecondsFromAUnixTimeStamp(response.Headers.FirstOrDefault(t => t.Name.Equals("X-Rate-Limit-Reset"))?.Value);
                }
            }
            while (response.StatusCode.Equals((HttpStatusCode)TOO_MANY_REQUESTS));

            return response;

        }

        public int GetMilisecondsFromAUnixTimeStamp(object unixTimpeStamp)
        {
            long unixTimpeStampLong;
            long.TryParse(unixTimpeStamp.ToString(), out unixTimpeStampLong) ;
            return (int)(unixTimpeStampLong - DateTimeOffset.Now.ToUnixTimeSeconds());
        }
    }
}
