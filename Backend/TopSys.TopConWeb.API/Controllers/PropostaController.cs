using LinqKit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Proposta.Inclusao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Infra.Reports;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class PropostaController : BaseController
    {
        private readonly IBombaPrecoApplicationService _bombaPrecoAppService;
        private readonly IParametroApplicationService _parametroAppService;
        private readonly IPropostaApplicationService _propostaAppService;
        private readonly IPropostaPropagandaApplicationService _propostaPropagandaApplicationService;
        private readonly IIntervenienteApplicationService _intervenienteApplicationService;
        private readonly ReportService _reportService;

        private readonly IObraApplicationService _obraApplicationService;
        private readonly ITracoPrecoApplicationService _tracoPrecoAppService;

        public PropostaController(
            IPropostaApplicationService propostaAppService, 
            IPropostaPropagandaApplicationService propostaPropagandaApplicationService,
            IIntervenienteApplicationService intervenienteApplicationService,
            ReportService reportService,
            IObraApplicationService obraApplicationService,
            IParametroApplicationService parametroAppService,
            IBombaPrecoApplicationService bombaPrecoAppService,
            ITracoPrecoApplicationService tracoPrecoAppService)
        {
            _propostaAppService = propostaAppService;
            _propostaPropagandaApplicationService = propostaPropagandaApplicationService;
            _intervenienteApplicationService = intervenienteApplicationService;
            _reportService = reportService;
            _obraApplicationService = obraApplicationService;
            _parametroAppService = parametroAppService;
            _bombaPrecoAppService = bombaPrecoAppService;
            _tracoPrecoAppService = tracoPrecoAppService;
        }

        [HttpPost]
        [Route("v1/proposta")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] PropostaInclusaoRequest proposta)
        {
            var parametroProposta = _parametroAppService.ObterPorDataBase<ParametroProposta>(DateTime.Now);

            if (proposta.Email.Equals(""))
            {
                
                if (proposta.StatusProposta == EPropostaStatusCliente.Aprovado && (proposta.IntervenienteTipo != "F" || parametroProposta.ObrigaEmailPessoaFisica))
                    return CreateResponse(HttpStatusCode.BadRequest, new { error = "E-mail do cliente é obrigatório!" });
            }

            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<IHasVendedor>("VendedorCodigo");

            var result = _propostaAppService.Adicionar(User.Identity.Name, proposta, filtroVendedores);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPatch]
        [Route("v1/proposta")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] PropostaAlteracaoRequest proposta)
        {
            if (proposta.Origem == "proposta")
            {
                var obra = _obraApplicationService.ObterPorId(proposta.Usina.Codigo, proposta.Obra.Numero);

                if (obra.NumContrato != 0)
                    return CreateResponse(HttpStatusCode.OK, $"Atualização não permitida através da proposta, contrato já gerado!");

            }

            var parametroProposta = _parametroAppService.ObterPorDataBase<ParametroProposta>(DateTime.Now);

            if (proposta.Email.Equals(""))
            {
                if (proposta.StatusProposta == EPropostaStatusCliente.Aprovado && (proposta.IntervenienteTipo != "F" || parametroProposta.ObrigaEmailPessoaFisica))
                    return CreateResponse(HttpStatusCode.BadRequest, new { error = "E-mail do cliente é obrigatório!" });
            }

            if (proposta.Obra.ObraBombas.Count > 0)
            {
                foreach (var bomba in proposta.Obra.ObraBombas)
                {
                    if (bomba.BombaPropria && proposta.Obra.UsinaEntrega != null && bomba.BombaTipo != null)
                    {
                        var precoBomba = _bombaPrecoAppService.ObterPorUsinaBombaTipoData(proposta.Obra.UsinaEntrega.Codigo, bomba.BombaTipo.Codigo, DateTime.Now);

                        if (precoBomba != null)
                        {
                            if (bomba.TipoCalculo != precoBomba.TipoCalculo)
                                return CreateResponse(HttpStatusCode.BadRequest, new { error = "Tipo de cálculo de bomba alterado! Edite e atualize o valor de bomba para recálculo do valor!" });
                        }
                    }
                    else if (!bomba.BombaPropria && bomba.Terceiro != null && bomba.BombaTipo != null)
                    {
                        var precoBombaTerceiro = _bombaPrecoAppService.ObterPorBombistaBombaTipoData(bomba.Terceiro.Codigo, bomba.BombaTipo.Codigo, DateTime.Now);

                        if (precoBombaTerceiro != null)
                        {
                            if (bomba.TipoCalculo != precoBombaTerceiro.TipoCalculo)
                                return CreateResponse(HttpStatusCode.BadRequest, new { error = "Tipo de cálculo de bomba alterado! Edite e atualize o valor de bomba para recálculo do valor!" });
                        }
                    }
                }
            }

            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<IHasVendedor>("VendedorCodigo");

            _propostaAppService.Atualizar(User.Identity.Name, proposta, filtroVendedores);

            return CreateResponse(HttpStatusCode.OK, $"{(proposta.Obra.NumContrato == 0 ? "Proposta atualizada" : "Contrato atualizado")} com sucesso!");
        }

        [HttpGet]
        [Route("v1/propostas")]
        [Authorize]
        public Task<HttpResponseMessage> ListarEmOrdemDecrescente([FromUri] PropostaPagedRequest request)
        {
            var filtroVendedores = User.Identity.FiltroVendedoresPermitidos<Proposta>("VendedorCodigo");
            
            var urlFilter = UrlFilterParser.Parse<Proposta>(request.Filter);

            var urlFilterExibicaoContrato = urlFilter.ExibicaoContratosFilter((EExibicaoContratos)request.FiltroExibicaoContratos);

            urlFilter = urlFilter.StatusPropostaClienteFilter((EPropostaStatusCliente)request.FiltroStatusProposta);
            
            var filter = filtroVendedores.And(t => t.UsinaCodigo == 999).And(urlFilter).And(urlFilterExibicaoContrato);
            var pagedList = _propostaAppService.ListarEmOrdemDecrescente(request.Pagina, request.PorPagina, filter, request.FiltroDivergenciaCarteira, request.StatusClicksignDocumento);
            
            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/propostas/cpf-cnpj/{cpfCnpj}/pagina/{pagina}/por-pagina/{porPagina}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorCpfCnpj(string cpfCnpj, int pagina, int porPagina)
        {
            var pagedList = _propostaAppService.ListarPorCpfCnpj(cpfCnpj, pagina, porPagina);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/proposta/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var proposta = _propostaAppService.ObterPorUsinaAnoNumero(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, proposta);
        }

        [HttpGet]
        [Route("v1/proposta/usina/{idUsina}/ano/{ano}/numero/{numero}/volume-total")]
        [Authorize]
        public Task<HttpResponseMessage> ObterVolumeTotalProposto(int idUsina, int ano, int numero)
        {
            var volumeTotal = _propostaAppService.ObterVolumeTotalProposto(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, volumeTotal);
        }

        [HttpGet]
        [Route("v1/proposta/usina/{idUsina}/ano/{ano}/numero/{numero}/report")]
        //[Authorize]
        public HttpResponseMessage ObterReportPorUsinaAnoNumero(int idUsina, int ano, int numero)
        {
            var report = _reportService.GetPropostaReport(idUsina, ano, numero);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/proposta/usina/{idUsina}/ano/{ano}/numero/{numero}/sequencia-programacao/{sequenciaProgramacao}/report")]
        //[Authorize]
        public HttpResponseMessage ObterReportPorUsinaAnoNumeroSequenciaProgramacao(int idUsina, int ano, int numero, int sequenciaProgramacao)
        {
            var report = _reportService.GetPropostaProgramacaoReport(idUsina, ano, numero, sequenciaProgramacao);

            return CreatePdfResponse(HttpStatusCode.OK, report);
        }

        [HttpGet]
        [Route("v1/proposta/report/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPropostaReportPDFSequencias(int idUsina, int ano, int numero)
        {
            var result = _propostaAppService.ListarPropostaReportPDFSequencias(idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/proposta/report/usina/{idUsina}/ano/{ano}/numero/{numero}")]
        [Authorize]
        public Task<HttpResponseMessage> CriarNovaPropostaReportPDF(int idUsina, int ano, int numero, [FromBody] PropostaReportNovoRequest request)
        {
            var result = _propostaAppService.CriarNovaPropostaReportPDF(idUsina, ano, numero, User.Identity.Name, request.PropagandaId);

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/proposta/report/usina/{idUsina}/ano/{ano}/numero/{numero}/sequencia/{sequencia}")]
        //[Authorize]
        public HttpResponseMessage ObterPropostaReportPDF(int idUsina, int ano, int numero, int sequencia)
        {
            var result = _propostaAppService.ObterPropostaReportPDF(idUsina, ano, numero, sequencia);

            return CreatePdfResponse(HttpStatusCode.OK, result);
        }

        #region Propaganda

        [HttpGet]
        [Route("v1/proposta/propaganda")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPropagandaTodos()
        {

            var result = _propostaPropagandaApplicationService.ListarTodos();

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("v1/proposta/propaganda/anexo/{id}")]
        [Authorize]
        public string ObterPropagandaAnexo(string id)
        {
            var result = _propostaPropagandaApplicationService.ObterAnexo(Guid.Parse(id));
            var propaganda = _propostaPropagandaApplicationService.ObterPorId(Guid.Parse(id));

            return _intervenienteApplicationService.ObterAnexoConvertidoBase64(result, propaganda.Nome);
        }

        [HttpGet]
        [Route("v1/proposta/propaganda/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPropaganda(string id)
        {
            var result = _propostaPropagandaApplicationService.ObterPorId(Guid.Parse(id));

            return CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("v1/proposta/propaganda")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarPropaganda(PropostaPropagandaAdicionarRequest request)
        {
            var usuario = User.Identity.Name;

            _propostaPropagandaApplicationService.Adicionar(request, usuario, out string mensagem);

            if (mensagem == "")
                return CreateResponse(HttpStatusCode.OK, "Anexo inserido com sucesso!");

            return CreateResponse(HttpStatusCode.BadRequest, mensagem);
        }

        [HttpPatch]
        [Route("v1/proposta/propaganda")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarPropaganda(PropostaPropagandaAtualizarRequest request)
        {
            _propostaPropagandaApplicationService.Atualizar(request);

            return CreateResponse(HttpStatusCode.OK, "Propaganda Ativa!");
        }

        [HttpDelete]
        [Route("v1/proposta/propaganda/{id}")]
        [Authorize]
        public Task<HttpResponseMessage> RemoverPropaganda(string id)
        {
            _propostaPropagandaApplicationService.Remover(Guid.Parse(id));

            return CreateResponse(HttpStatusCode.OK, "Removido com sucesso.");
        }

        #endregion

    }
}