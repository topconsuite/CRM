using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Configuration;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.API.Telemetry
{
    public class TenantTelemetryProcessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;
        private readonly string _tenantId;
        private readonly string _tenantName;

        public TenantTelemetryProcessor(ITelemetryProcessor next)
        {
            _next = next;
            //_tenantId = parametroService.ObterParametroN("Telluria", "ClienteCodigo");
            //_tenantName = parametroService.ObterParametroN("Telluria", "ClienteNome");
        }

        public void Process(ITelemetry item)
        {
            item.Context.GlobalProperties["ClienteTelluriaCodigo"] = "4545";
            item.Context.GlobalProperties["ClienteTelluriaNome"] = "CORTESIA";

            _next.Process(item);
        }
    }
}