using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class RemessaController : BaseController
    {
        private readonly IRemessaApplicationService _remessaApplicationService;

        public RemessaController(IRemessaApplicationService remessaApplicationService)
        {
            _remessaApplicationService = remessaApplicationService;
        }

        [HttpGet]
        [Route("integrations/packing-lists/{contratoUsina}/{contratoAno}/{contratoNumero}/{programacaoSequencia}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> RemessaObterPorProgramacao([FromUri] int contratoUsina, int contratoAno, int contratoNumero, int programacaoSequencia)
        {
            var result = _remessaApplicationService.ObterPorProgramacao(contratoUsina, contratoAno, contratoNumero, programacaoSequencia);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/packing-lists/{filial}/{interveniente}/{tipoDocumento}/{serie}/{numero}/{sequencia}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> RemessaObterPorId([FromUri] int filial, int interveniente, int tipoDocumento, string serie, long numero, int sequencia)
        {
            var result = _remessaApplicationService.ObterPorId(filial, interveniente, tipoDocumento, serie, numero, sequencia);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/packing-lists/{filial}/{tipoDocumento}/{serie}/{numero}/{sequencia}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> RemessaObterPorIdSemInterveniente([FromUri] int filial, int tipoDocumento, string serie, long numero, int sequencia)
        {
            var result = _remessaApplicationService.ObterPorIdSemInterveniente(filial, tipoDocumento, serie, numero, sequencia);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/packing-lists/{filial}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> RemessaObterPorCentralEPeriodo(
            [FromUri] int filial, 
            [Required] DateTime start_date,
            DateTime? end_date = null,
            int page = 1,
            int limit = 10)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _remessaApplicationService.ObterPorCentralEPeriodo(filial, start_date, end_date, page, limit);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/packing-lists/by-update-date")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> RemessaObterPorDataAtualizacao([FromUri] [Required] DateTime start_date, DateTime? end_date = null, int page = 1, int limit = 10)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _remessaApplicationService.ObterPorDataAtualizacao(start_date, end_date, page, limit);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/packing-lists/by-automation-return-date")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> RemessaObterPorDataRetornoAutomacao([FromUri] [Required] DateTime start_date, DateTime? end_date = null, int page = 1, int limit = 10)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _remessaApplicationService.ObterPorDataRetornoAutomacao(start_date, end_date, page, limit);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/packing-lists/by-referrer")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ObterPontosPorIndicacao([FromUri] DateTime? start_date = null, DateTime? end_date = null, int seller = 0, string referrer_name = "", int page = 1, int limit = 10)
        {

            if(start_date != null && start_date.Value.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _remessaApplicationService.ObterIndicadorPontos(start_date, end_date, seller, referrer_name, page, limit);

            return CreateResponse(result);

        }
    }
}