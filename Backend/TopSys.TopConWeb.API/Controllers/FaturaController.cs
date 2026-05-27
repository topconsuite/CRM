using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Request.Fatura;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class FaturaController : BaseController
    {
        private readonly IFaturaApplicationService _faturaApplicationService;

        public FaturaController(IFaturaApplicationService faturaApplicationService)
        {
            _faturaApplicationService = faturaApplicationService;
        }

        [HttpGet]
        [Route("integrations/invoice/{branch}/{client}/{documentType}/{invoiceNumber}/{invoiceSeries}/{invoiceSubSeries}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FaturaObterPorChave([FromUri] int branch, int client, int documentType, long invoiceNumber, string invoiceSeries, int invoiceSubSeries)
        {
            var result = _faturaApplicationService.ObterPorChave(branch, client, documentType, invoiceNumber, invoiceSeries, invoiceSubSeries);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/invoice")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FaturaListar(
            [FromUri(Name = "invoice-date")] DateTime? invoiceDate = null,
            int? branch = null,
            [FromUri(Name = "cost-center")] int? costCenter = null,
            [FromUri(Name = "document-type")] int? documentType = null,
            int? client = null,
            int? page = null,
            int? limit = null)
        {
            if (invoiceDate != null && invoiceDate.Value.TimeOfDay != TimeSpan.Zero)
                invoiceDate = DateTime.SpecifyKind(invoiceDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _faturaApplicationService.Listar(invoiceDate, branch, costCenter, documentType, client, page, limit);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/invoice/by-update-date")]
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

            var result = _faturaApplicationService.ObterPorDataAtualizacao(start_date, end_date, page, limit);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/invoice/{branch}/{client}/{documentType}/{invoiceNumber}/{invoiceSeries}/{invoiceSubSeries}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> FaturaAtualizar([FromUri] int branch, int client, int documentType, long invoiceNumber, string invoiceSeries, int invoiceSubSeries,
            [FromBody] [Required] FaturaAtualizarRequest request)
        {
            var result = _faturaApplicationService.FaturaAtualizarPorChave(branch, client, documentType, invoiceNumber, invoiceSeries, invoiceSubSeries,request);
            return CreateResponse(result);
        }
        
    }
}