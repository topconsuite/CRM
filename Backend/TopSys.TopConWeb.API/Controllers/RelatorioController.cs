using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Domain.Entities;
using System.Linq.Expressions;
using LinqKit;
using TopSys.TopConWeb.Application.DTOS.Request.Relatorio.RelatorioProducao;
using TopSys.TopConWeb.Infra.Reports;
using TopSys.TopConWeb.Infra.Reports.FilterClasses;
using System.IO;
using System.Text;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class RelatorioController : BaseController
    {
        private readonly ReportService _reportService;

        public RelatorioController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("v1/relatorio/producao-analitico")]
        //[Authorize]
        public HttpResponseMessage ObterRelatorioProducaoAnalitico([FromUri] RelatorioProducaoRequest request, [FromUri] bool detalharAdicionais, [FromUri] bool detalharViaCaptacao)
        {
            byte[] data = Convert.FromBase64String(request.Filter);
            var filter = Encoding.UTF8.GetString(data);
            var filtro = UrlFilterParser.Convert<RelatorioProducaoFilter>(filter);

            var report = _reportService.GetRelatorioProducaoAnaliticoReport(filtro, detalharAdicionais, detalharViaCaptacao);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/relatorio/producao-sintetico")]
        //[Authorize]
        public HttpResponseMessage ObterRelatorioProducaoSintetico([FromUri] RelatorioProducaoRequest request)
        {
            byte[] data = Convert.FromBase64String(request.Filter);
            var filter = Encoding.UTF8.GetString(data);
            var filtro = UrlFilterParser.Convert<RelatorioProducaoFilter>(filter);

            var report = _reportService.GetRelatorioProducaoSinteticoReport(filtro);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/relatorio/producao-por-programacao")]
        //[Authorize]
        public HttpResponseMessage ObterRelatorioProducaoPorProgramacao([FromUri] RelatorioProducaoRequest request, [FromUri] bool detalharViaCaptacao)
        {
            byte[] data = Convert.FromBase64String(request.Filter);
            var filter = Encoding.UTF8.GetString(data);
            var filtro = UrlFilterParser.Convert<RelatorioProducaoFilter>(filter);

            var report = _reportService.GetRelatorioProducaoPorProgramacaoReport(filtro, detalharViaCaptacao);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/relatorio/producao-volume")]
        //[Authorize]
        public HttpResponseMessage ObterRelatorioProducaoVolume([FromUri] RelatorioProducaoRequest request)
        {
            byte[] data = Convert.FromBase64String(request.Filter);
            var filter = Encoding.UTF8.GetString(data);
            var filtro = UrlFilterParser.Convert<RelatorioProducaoFilter>(filter);

            var report = _reportService.GetRelatorioVolumeReport(filtro);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }
    }
}