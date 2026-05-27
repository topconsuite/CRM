using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class NotaFiscalDigitalController : BaseController
    {
        private readonly INotaFiscalDigitalApplicationService _notaFiscalDigitalApplicationService;

        public NotaFiscalDigitalController(INotaFiscalDigitalApplicationService notaFiscalDigitalApplicationService)
        {
            _notaFiscalDigitalApplicationService = notaFiscalDigitalApplicationService;
        }

        [HttpGet]
        [Route("integrations/digital-invoice/{branch}/{client}/{documentType}/{series}/{invoiceNumber}/{invoiceSequence}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> NotaFiscalDigitalObterPorChave([FromUri] int branch, int client, int documentType, string series, long invoiceNumber, int invoiceSequence)
        {
            var result = _notaFiscalDigitalApplicationService.ObterPorChave(branch, client, documentType, series, invoiceNumber, invoiceSequence);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/digital-invoice/{branch}/{documentType}/{series}/{invoiceNumber}/{invoiceSequence}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> NotaFiscalDigitalObterPorChaveSemInterveniente([FromUri] int branch, int documentType, string series, long invoiceNumber, int invoiceSequence)
        {
            var result = _notaFiscalDigitalApplicationService.ObterPorChaveSemInterveniente(branch, documentType, series, invoiceNumber, invoiceSequence);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/digital-invoice")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> NotaFiscalDigitalListar(
            [FromUri(Name = "invoice-date")] DateTime? invoiceDate = null,
            int? branch = null,
            [FromUri(Name = "document-type")] int? documentType = null,
            [FromUri(Name = "cost-center")] int? centroCusto = null,
            int? client = null,
            int? page = null,
            int? limit = null)
        {
             if (invoiceDate != null && invoiceDate.Value.TimeOfDay != TimeSpan.Zero)
                invoiceDate = DateTime.SpecifyKind(invoiceDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _notaFiscalDigitalApplicationService.Listar(invoiceDate, branch, documentType, centroCusto, client, page, limit);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/digital-invoice/by-update-date")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> NotaFiscalDigitalObterPorDataAtualizacao(
            [FromUri][Required] DateTime start_date,
            DateTime? end_date = null,
            int? page = null,
            int? limit = null)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _notaFiscalDigitalApplicationService.ObterPorDataAtualizacao(start_date, end_date, page, limit);

            return CreateResponse(result);
        }
    }
}