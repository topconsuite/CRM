using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Domain.Entities;
using LinqKit;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Alteracao;
using TopSys.TopConWeb.Infra.Reports;
using TopSys.TopConWeb.Application.DTOS.Response.Programacao.ProgramacaoSimplesResponse;
using System.Linq;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Gerar;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Rejeitar;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.Programacao.Integracao;
using System.ComponentModel.DataAnnotations;
using System;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ProgramacaoController : BaseController
    {
        private readonly IProgramacaoApplicationService _programacaoApplicationService;
        private readonly ReportService _reportService;

        public ProgramacaoController(IProgramacaoApplicationService programacaoApplicationService, ReportService reportService)
        {
            _programacaoApplicationService = programacaoApplicationService;
            _reportService = reportService;
        }

        [HttpPost]
        [Route("v1/programacao")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] ProgramacaoInclusaoRequest programacao)
        {
            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<Proposta>("VendedorCodigo");

            _programacaoApplicationService.Adicionar(User.Identity.Name, programacao);

            return CreateResponse(HttpStatusCode.OK, "Programação inserida com sucesso!");
        }

        [HttpPatch]
        [Route("v1/programacao")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] ProgramacaoAlteracaoRequest programacao)
        {
            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<Proposta>("VendedorCodigo");

            _programacaoApplicationService.Atualizar(User.Identity.Name, programacao);

            return CreateResponse(HttpStatusCode.OK, "Programação atualizada com sucesso!");
        }

        [HttpGet]
        [Route("v1/programacoes/desc")]
        [Authorize]
        public Task<HttpResponseMessage> ListarComPropostaContratoEmOrdemDescrescente([FromUri] ProgramacaoPagedRequest request)
        {
            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<Programacao>("Proposta.VendedorCodigo");

            var urlFilter = UrlFilterParser.Parse<Programacao>(request.Filter);

            var filter = filtroVendedores.And(t => t.UsinaCodigo == 999).And(urlFilter).And(t => t.PropostaNumero > 0);
            
            var pagedList = _programacaoApplicationService.ListarComPropostaContratoEmOrdemDescrescente(request.Pagina, request.PorPagina, filter);
            
            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/programacoes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarComPropostaContratoEmOrdemCrescente([FromUri] ProgramacaoPagedRequest request)
        {
            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<Programacao>("Proposta.VendedorCodigo");

            var urlFilter = UrlFilterParser.Parse<Programacao>(request.Filter);

            var filter = filtroVendedores.And(t => t.UsinaCodigo == 999).And(urlFilter).And(t => t.PropostaNumero > 0);

            var pagedList = _programacaoApplicationService.ListarComPropostaContratoEmOrdemCrescente(request.Pagina, request.PorPagina, filter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/programacoes/usina/{idUsina}/proposta-ano/{propostaAno}/proposta-numero/{propostaNumero}/obra-numero/{obraNumero}/obra-traco-sequencia/{obraTracoSequencia}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorPropostaTraco(int idUsina, int propostaAno, int propostaNumero, int obraNumero, int obraTracoSequencia)
        {
            var programacoes = _programacaoApplicationService.ListarFiltrados<ProgramacaoSimplesResponse>(t =>
                t.UsinaCodigo == idUsina
                && t.PropostaAno == propostaAno
                && t.PropostaNumero == propostaNumero
                && t.ObraNumero == obraNumero
                && t.ObraTracoSequencia == obraTracoSequencia
            );

            return CreateResponse(HttpStatusCode.OK, programacoes);
        }

        [HttpGet]
        [Route("v1/programacoes/usina/{idUsina}/proposta-ano/{propostaAno}/proposta-numero/{propostaNumero}/obra-numero/{obraNumero}/obra-bomba-sequencia/{obraBombaSequencia}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorPropostaBomba(int idUsina, int propostaAno, int propostaNumero, int obraNumero, int obraBombaSequencia)
        {
            var programacoes = _programacaoApplicationService.ListarFiltrados<ProgramacaoSimplesResponse>(t =>
                t.UsinaCodigo == idUsina
                && t.PropostaAno == propostaAno
                && t.PropostaNumero == propostaNumero
                && t.ObraNumero == obraNumero
                && t.ObraBombaSequencia == obraBombaSequencia
            );

            return CreateResponse(HttpStatusCode.OK, programacoes);
        }

        [HttpGet]
        [Route("v1/programacoes/usina/{idUsina}/obra-numero/{obraNumero}/demais-servicos-sequencia/{demaisServicosSequencia}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorItemDemaisServicos(int idUsina, int obraNumero, int demaisServicosSequencia)
        {
            var demaisServicos = _programacaoApplicationService.ListarFiltrados<ProgramacaoDemaisServicos, ProgramacaoDemaisServicos>(t =>
                t.UsinaCodigo == idUsina
                && t.ObraNumero == obraNumero
                && t.Sequencia == demaisServicosSequencia
            );

            var progSeqs = demaisServicos.Select(t => t.ProgramacaoSequencia).Distinct().ToArray();

            var programacoes = _programacaoApplicationService.ListarFiltrados<ProgramacaoSimplesResponse>(t =>
                t.UsinaCodigo == idUsina
                && t.ObraNumero == obraNumero
                && progSeqs.Contains(t.Sequencia)
            );

            return CreateResponse(HttpStatusCode.OK, programacoes);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterDetalhadaPorId(int idUsina, int obraNumero, int sequencia)
        {
            var programacao = _programacaoApplicationService.ObterDetalhadaPorId(idUsina, obraNumero, sequencia);

            return CreateResponse(HttpStatusCode.OK, programacao);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/tem-nf-emitida")]
        [Authorize]
        public Task<HttpResponseMessage> TemNotaFiscalEmitida(int idUsina, int obraNumero, int sequencia)
        {
            var temNF = _programacaoApplicationService.TemNotaFiscalEmitida(idUsina, obraNumero, sequencia);

            return CreateResponse(HttpStatusCode.OK, temNF);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/contrato-ano/{contratoAno}/contrato-numero/{contratoNumero}/sequencia/{sequencia}/horarios")]
        [Authorize]
        public Task<HttpResponseMessage> ListarHorarios(int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            var horarios = _programacaoApplicationService.ListarHorarios(idUsina, contratoAno, contratoNumero, sequencia);

            return CreateResponse(HttpStatusCode.OK, horarios);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/voume-total")]
        [Authorize]
        public Task<HttpResponseMessage> ObterVolumeTotalProgramado(int idUsina, int obraNumero)
        {
            var volumeTotal = _programacaoApplicationService.ObterVolumeTotalProgramado(idUsina, obraNumero);

            return CreateResponse(HttpStatusCode.OK, volumeTotal);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/logs")]
        [Authorize]
        public Task<HttpResponseMessage> ListarProgramacaoLogsPorId(int idUsina, int obraNumero, int sequencia)
        {
            var programacaoLogs = _programacaoApplicationService.ListarProgramacaoLogsPorId(idUsina, obraNumero, sequencia);

            return CreateResponse(HttpStatusCode.OK, programacaoLogs);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/bomba")]
        [Authorize]
        public Task<HttpResponseMessage> ObterBomba(int idUsina, int obraNumero, int sequencia)
        {
            return CreateResponse(
                HttpStatusCode.OK, 
                _programacaoApplicationService.ObterBombaDaProgramacao(idUsina, obraNumero, sequencia)
                );
        }

        [HttpPatch]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/cancelar/observacao/{observacao}")]
        [Authorize]
        public Task<HttpResponseMessage> CancelarPorId(int idUsina, int obraNumero, int sequencia, string observacao)
        {
            _programacaoApplicationService.CancelarPorId(User.Identity.Name, idUsina, obraNumero, sequencia, observacao);

            return CreateResponse(HttpStatusCode.OK, "Programação cancelada com sucesso!");
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/report")]
        //[Authorize]
        public HttpResponseMessage ObterReportPorUsinaAnoNumero(int idUsina, int obraNumero, int sequencia)
        {
            var report = _reportService.GetProgramacaoReport(idUsina, obraNumero, sequencia);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpPatch]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/gerar")]
        [Authorize]
        public Task<HttpResponseMessage> GeraProgramacao(int idUsina, int obraNumero, int sequencia, ProgramacaoGerarRequest request)
        {
            var result= _programacaoApplicationService.GeraProgramacao(idUsina, obraNumero, sequencia, request.atualizaComplexidadeBombeado, request.gravaContinuidadeProgramacao, User.Identity.Name);

            if (!result) return CreateResponse(HttpStatusCode.NotAcceptable, result);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/complexidade-bombeado")]
        [Authorize]
        public Task<HttpResponseMessage> TemComplexidadeBombeado(int idUsina, int obraNumero, int sequencia)
        {
            var result  = _programacaoApplicationService.TemComplexidadeBombeado(idUsina, obraNumero, sequencia);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/verifica-continuidade")]
        [Authorize]
        public Task<HttpResponseMessage> VerificaContinuidade(int idUsina, int obraNumero, int sequencia)
        {
            var result = _programacaoApplicationService.VerificaContinuidade(idUsina, obraNumero, sequencia);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPatch]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/rejeitar")]
        [Authorize]
        public Task<HttpResponseMessage> RejeitaProgramacao(int idUsina, int obraNumero, int sequencia, ProgramacaoRejeitarRequest request)
        {
            var result = _programacaoApplicationService.RejeitaProgramacao(idUsina, obraNumero, sequencia, request.observacaoRejeitada, User.Identity.Name);

            if (!result) return CreateResponse(HttpStatusCode.NotAcceptable, result);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPatch]
        [Route("v1/programacao/usina/{idUsina}/obra-numero/{obraNumero}/sequencia/{sequencia}/aprovacao-financeira")]
        [Authorize]
        public Task<HttpResponseMessage> AprovaFinanceiro(int idUsina, int obraNumero, int sequencia)
        {
            _programacaoApplicationService.AprovaFinanceiro(idUsina, obraNumero, sequencia, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Aprovação Financeira realizada com Sucesso!");
        }

        #region Integracao

        [HttpPost]
        [Route("integrations/concretings")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ProgramacaoAdicionar([FromBody] ProgramacoesRequest request)
        {
            var result = _programacaoApplicationService.Adicionar(request.Programacoes);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/concretings/{idUsina}/{contratoAno}/{contratoNumero}/{sequencia}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ProgramacaoAtualizarPorId([FromUri] int idUsina, int contratoAno, int contratoNumero, int sequencia, [FromBody] ProgramacaoAtualizarRequest request)
        {
            var result = _programacaoApplicationService.AtualizarPorId(idUsina, contratoAno, contratoNumero, sequencia, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/concretings/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ProgramacaoAtualizarPorExternalId([FromUri] string external_id, [FromBody] ProgramacaoAtualizarRequest request)
        {
            var result = _programacaoApplicationService.AtualizarPorExternalId(external_id, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/concretings/{usina}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ProgramacaoObterPorCentralEPeriodo(
            [FromUri] int usina,
            [Required] DateTime start_date,
            DateTime? end_date = null)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _programacaoApplicationService.ObterPorUsinaEPeriodo(usina, start_date, end_date);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/concretings/{idUsina}/{contratoAno}/{contratoNumero}/{sequencia}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ProgramacaoObterPorID([FromUri] int idUsina, int contratoAno, int contratoNumero, int sequencia)
        {
            var result = _programacaoApplicationService.ObterPorId(idUsina, contratoAno, contratoNumero, sequencia);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/concretings/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ProgramacaoObterPorExternalId([FromUri] string external_id)
        {
            var result = _programacaoApplicationService.ObterPorExternalId(external_id);

            return CreateResponse(result);
        }

        #endregion
    }
}