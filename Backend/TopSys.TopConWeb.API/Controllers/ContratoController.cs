using LinqKit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.AprovarCoincidenciasCadastraisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoIntegracao;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ReprovarCoincidenciasCadastraisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoGeradoResponse;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Infra.Reports;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ContratoController : BaseController
    {
        private readonly IContratoApplicationService _contratoApplicationService;
        private readonly IPropostaApplicationService _propostaApplicationService;
        private readonly ReportService _reportService;

        public ContratoController(IContratoApplicationService contratoApplicationService, IPropostaApplicationService propostaApplicationService, ReportService reportService)
        {
            _contratoApplicationService = contratoApplicationService;
            _propostaApplicationService = propostaApplicationService;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("v1/contrato/revalidacaoCadastro")]
        [Authorize]
        public Task<HttpResponseMessage> ListarContratosRevalidacaoCadastro()
        {
            var contratosRevalidacaoCadastro = _contratoApplicationService.ListarContratosRevalidacaoCadastro();

            return CreateResponse(HttpStatusCode.OK, contratosRevalidacaoCadastro);
        }

        [HttpPost]
        [Route("v1/contrato/revalidacaoCadastro/aprovar")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarContratoRevalidacaoCadastro(ContratoRevalidacaoCadastroRequest contratoRevalidacaoCadastro)
        {
            string mensagem = "";

            _contratoApplicationService.AprovarContratoRevalidacaoDeCadastro(User.Identity.Name, contratoRevalidacaoCadastro, ref mensagem);

            return CreateResponse(HttpStatusCode.OK, mensagem);
        }

        [HttpPost]
        [Route("v1/contrato/revalidacaoCadastro/aprovar/via-integracao-limite-credito")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> AprovarContratoRevalidacaoCadastroViaIntegracaoLimiteCredito(ContratoRevalidacaoCadastroRequest contratoRevalidacaoCadastro)
        {
            string mensagem = "";

            _contratoApplicationService.AprovarContratoRevalidacaoDeCadastro("AUTO", contratoRevalidacaoCadastro, ref mensagem);

            return CreateResponse(HttpStatusCode.OK, mensagem);
        }

        [HttpPost]
        [Route("v1/contrato/proposta-usina/{propostaUsina}/proposta-ano/{propostaAno}/proposta-numero/{propostaNumero}/gerar")]
        [Authorize]
        public Task<HttpResponseMessage> GerarContrato(int propostaUsina, int propostaAno, int propostaNumero)
        {
            var success = _contratoApplicationService.GerarContrato(User.Identity.Name, propostaUsina, propostaAno, propostaNumero, out ContratoGeradoResponse contrato, out string mensagem);

            if (success)
                return CreateResponse(HttpStatusCode.OK, contrato);

            return CreateResponse(HttpStatusCode.BadRequest, mensagem);
        }

        [HttpPost]
        [Route("v1/contrato/proposta-usina/{propostaUsina}/proposta-ano/{propostaAno}/proposta-numero/{propostaNumero}/gerar-comercial")]
        [Authorize]
        public Task<HttpResponseMessage> GerarEValidarContrato(int propostaUsina, int propostaAno, int propostaNumero)
        {
            var success = _contratoApplicationService.GerarEValidarContrato(User.Identity.Name, propostaUsina, propostaAno, propostaNumero, out ContratoGeradoResponse contrato, out string mensagem);

            if (success)
                return CreateResponse(HttpStatusCode.OK, contrato);

            return CreateResponse(HttpStatusCode.BadRequest, mensagem);
        }

        [HttpGet]
        [Route("v1/contrato/usina/{idUsina}/ano/{ano}/numero/{numero}/report")]
        //[Authorize]
        public HttpResponseMessage ObterReportPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var report = _reportService.GetContratoReport(idUsina, ano, numero, 1);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/contratoResidual/usina/{idUsina}/ano/{ano}/numero/{numero}/report")]
        //[Authorize]
        public HttpResponseMessage ObterReportResidualPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var report = _reportService.GetContratoResidualReport(idUsina, ano, numero);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpPost]
        [Route("v1/contrato/cadastro/coincidencias/reprovar")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarCoincidenciasCadastrais(ReprovarCoincidenciasCadastraisRequest reprovaCoincidenciasCadastraisRequest)
        {
            _contratoApplicationService.ReprovarCoincidenciasCadastrais(User.Identity.Name, reprovaCoincidenciasCadastraisRequest);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("v1/contrato/cadastro/coincidencias/aprovar")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarCoincidenciasCadastrais(AprovarCoincidenciasCadastraisRequest aprovaCoincidenciasCadastraisRequest)
        {
             _contratoApplicationService.AprovarCoincidenciasCadastrais(User.Identity.Name, aprovaCoincidenciasCadastraisRequest);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/contrato/versoes/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarContratoVersoesAprovados(int idUsina, int ano, int numero)
        {
            var listarContratoVersoes = _contratoApplicationService.ListarContratoVersoesAprovados(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, listarContratoVersoes);
        }

        [HttpGet]
        [Route("v1/contrato/versao/{versao}/usina/{idUsina}/ano/{ano}/numero/{numero}/report")]
        //[Authorize]
        public HttpResponseMessage ObterPDFContratoVersao(int versao, int idUsina, int ano, int numero)
        {
            var report = _contratoApplicationService.ObterPDFContratoVersao(versao, idUsina, ano, numero);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/aditivo/versao/{versao}/usina/{idUsina}/anoprop/{anoproposta}/numeroprop/{numeroproposta}/anocontr/{anocontrato}/numerocontr/{numerocontrato}/report")]
        //[Authorize]
        public HttpResponseMessage ObterAditivoReport(int versao, int idUsina, int anoproposta, int numeroproposta, int anocontrato, int numerocontrato)
        {
            var report = _contratoApplicationService.ObterAditivoReport(versao, idUsina, anoproposta, numeroproposta, anocontrato, numerocontrato);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/contrato/versao")]
        [Authorize]
        public Task<HttpResponseMessage> ObterContratoVersao()
        {
            var parametros = _contratoApplicationService.ObterParametrosContratoVersao();

            return CreateResponse(HttpStatusCode.OK, parametros);
        }

        [HttpGet]
        [Route("v1/contratos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPropostasComContratosEmOrdemDecrescente([FromUri] PropostaPagedRequest request)
        {
            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<Proposta>("VendedorCodigo");

            var urlFilter = UrlFilterParser.Parse<Proposta>(request.Filter);

            var urlFilterExibicaoContrato = urlFilter.ExibicaoContratosFilter((EExibicaoContratos)request.FiltroExibicaoContratos);

            urlFilter = urlFilter.StatusPropostaClienteFilter((EPropostaStatusCliente)request.FiltroStatusProposta);

            var filter = filtroVendedores.And(t => t.UsinaCodigo == 999).And(urlFilter).And(urlFilterExibicaoContrato);
            var pagedList = _propostaApplicationService.ListarEmOrdemDecrescente(request.Pagina, request.PorPagina, filter, request.FiltroDivergenciaCarteira, request.StatusClicksignDocumento, true);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/contrato-finalidades")]
        [Authorize]
        public Task<HttpResponseMessage> ListarFinalidades()
        {
            return CreateResponse(HttpStatusCode.OK, _contratoApplicationService.ListarFinalidades());
        }

        #region API Contratos Simplicada 


        [HttpGet]
        [Route("integrations/simplified-contract/{idUsina}/{contratoAno}/{contratoNumero}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ContratoSimplificadoObterPorID([FromUri] int idUsina, int contratoAno, int contratoNumero)
        {
            var result = _contratoApplicationService.ObterPorId(idUsina, contratoAno, contratoNumero);

            return CreateResponse(result);
        }

        [HttpPost]
        [Route("integrations/simplified-contract/financial-approval")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> ContratoSimplificadoAprovacaoFinanceiraPorID([FromBody] PagamentosRequest pagamentos)
        {
            return CreateResponse(_contratoApplicationService.AprovacaoFinanceira(pagamentos));
        }

        #endregion
    }
}