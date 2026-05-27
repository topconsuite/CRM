using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente;
using TopSys.TopConWeb.Application.DTOS.Response.Interveniente.IntervenienteSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Request.Filtered;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using LinqKit;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente.AlterarLimiteCreditoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteHistorico;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteHistorico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Request.Interveniente;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System;
using TopSys.TopConWeb.API.Security;
using System.IO;
using TopSys.TopConWeb.Application.DTOS.Request.IntervenienteAnexo;
using System.Linq.Expressions;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class IntervenienteController: BaseController
    {

        private readonly IIntervenienteApplicationService _intervenienteAppService;

        private readonly IParametroApplicationService _parametroAppService;

        public IntervenienteController(IIntervenienteApplicationService intervenienteAppService, IParametroApplicationService parametroAppService)
        {
            _intervenienteAppService = intervenienteAppService;
            _parametroAppService = parametroAppService;
        }

        [HttpGet]
        
        [Route("v1/interveniente/cpfCnpj/{cpfCnpj}/inscricao-estadual/{inscricaoEstadual}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorCpfCnpj(string cpfCnpj, string inscricaoEstadual)
        {
            var interveniente = _intervenienteAppService.ObterPorCpfCnpj(cpfCnpj, inscricaoEstadual);

            return CreateResponse(HttpStatusCode.OK, interveniente);
        }

        [HttpGet]
        [Route("v1/interveniente-permitido/cpfCnpj/{cpfCnpj}/inscricao-estadual/{inscricaoEstadual}")]
        [Authorize]

        public Task<HttpResponseMessage> ObterPorCpfCnpjVendedoresPermitidos(string cpfCnpj, string inscricaoEstadual)
        {
            var acessoGeralListaClientes = _parametroAppService.ObterParametroN("web", "AcessoGeralListaClientesComVendedorExclusivo");

            Expression<Func<Interveniente, bool>> filtroBase = t => t.CpfCnpj.Equals(cpfCnpj) && t.Cliente.Equals("S");

            Expression<Func<Interveniente, bool>> filtroVendedor = User.Identity.FiltroVendedoresPermitidos<Interveniente>("VendedorCodigo")
                                                              .Or(t => t.VendedorCodigo == 0);

            Expression<Func<Interveniente, bool>> filtro;

            if (cpfCnpj.Length > 11) {
                if (acessoGeralListaClientes.Equals("0"))
                    filtro = filtroBase.And(filtroVendedor);
                else
                    filtro = filtroBase;

                var interveniente = _intervenienteAppService
                .ListarFiltrados<IntervenienteResponse>(filtro, t => t.EnderecoMunicipio, t => t.Vendedor, t => t.BloqueioMotivo, t => t.GrupoEconomico)
                .FirstOrDefault();
                return CreateResponse(HttpStatusCode.OK, interveniente);
            }
            else
            {
                filtroBase = filtroBase.And(t => t.InscricaoEstadual.Equals(inscricaoEstadual) || t.InscricaoEstadual.Equals(""));
                if (acessoGeralListaClientes.Equals("0"))
                    filtro = filtroBase.And(filtroVendedor);
                else
                    filtro = filtroBase;

                var interveniente = _intervenienteAppService
                .ListarFiltrados<IntervenienteResponse>(filtro, t => t.EnderecoMunicipio, t => t.Vendedor, t => t.BloqueioMotivo, t => t.GrupoEconomico)
                .FirstOrDefault();
                return CreateResponse(HttpStatusCode.OK, interveniente);
            }
        }

        [HttpGet]
        [Route("v1/intervenientes")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorNome([FromUri] FilteredRequest request)
        {
            var filterLambda = UrlFilterParser.Parse<Interveniente>(request!=null ? request.Filter : null );

            var intervenientes = _intervenienteAppService.ListarFiltrados<IntervenienteSimplesResponse>(filterLambda, t => t.GrupoEconomico);

            return CreateResponse(HttpStatusCode.OK, intervenientes);
        }

        [HttpGet]
        [Route("v1/intervenientes-permitidos")]
        [Authorize]
        public Task<HttpResponseMessage> ObterVendedoresPermitidos([FromUri] FilteredRequest request)
        {
            var acessoGeralListaClientes = _parametroAppService.ObterParametroN("web", "AcessoGeralListaClientesComVendedorExclusivo");

            var filterLambda = UrlFilterParser.Parse<Interveniente>(request != null ? request.Filter : null);

            Expression<Func<Interveniente, bool>> filtroVendedor = User.Identity.FiltroVendedoresPermitidos<Interveniente>("VendedorCodigo")
                                                  .Or(t => t.VendedorCodigo == 0);

            Expression<Func<Interveniente, bool>> filtro;

            if (acessoGeralListaClientes.Equals("0"))
                filtro = filterLambda.And(t => t.Cliente.Equals("S")).And(filtroVendedor);
            else
                filtro = filterLambda.And(t => t.Cliente.Equals("S"));

            var interveniente = _intervenienteAppService
                .ListarFiltrados<IntervenienteResponse>(filtro, t => t.EnderecoMunicipio, t => t.Vendedor, t => t.BloqueioMotivo, t => t.GrupoEconomico);

            return CreateResponse(HttpStatusCode.OK, interveniente);
        }

        [HttpGet]
        [Route("v1/intervenientes/inscricao-estadual/{inscricaoEstadual}/uf/{uf}/valida")]
        [Authorize]
        public Task<HttpResponseMessage> InscricaoEstadualEhValida(string inscricaoEstadual, string uf)
        {
            var isValid = _intervenienteAppService.InscricaoEstadualEhValida(inscricaoEstadual, uf);

            return CreateResponse(HttpStatusCode.OK, isValid);
        }

        [HttpPost]
        [Route("v1/intervenientes/aprovar-iss")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarIss([FromBody]int CodInterveniente)
        {
            _intervenienteAppService.AprovarIss(User.Identity.Name, CodInterveniente);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("v1/intervenientes/alterar-limite-credito")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarLimiteCredito(AlterarLimiteCreditoRequest alterarLimiteCreditoRequest)
        {
            _intervenienteAppService.AlterarLimiteDeCredito(alterarLimiteCreditoRequest);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/intervenientes/listar-historico/desc")]
        [Authorize]
        public Task<HttpResponseMessage> ListarHistoricoDescrescente([FromUri] IntervenienteHistoricoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<IntervenienteHistorico>(request.Filter);
            var intervenienteHistorico = _intervenienteAppService.ListarHistoricoEmOrdemDescrescente(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, intervenienteHistorico);
        }

        [HttpGet]
        [Route("v1/interveniente/codigo/{intervenienteCodigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorCodigo(int intervenienteCodigo)
        {
            var filterLambda = UrlFilterParser.Parse<Interveniente>($"$(Codigo=={intervenienteCodigo})");

            var interveniente = _intervenienteAppService
            .ListarFiltrados<IntervenienteResponse>(filterLambda, t => t.EnderecoMunicipio, t => t.Vendedor, t => t.BloqueioMotivo, t => t.GrupoEconomico)
            .FirstOrDefault();

            return CreateResponse(HttpStatusCode.OK, interveniente);
        }

        [HttpPost]
        [Route("v1/interveniente/historico")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] IntervenienteHistoricoRequest intervenienteHistorico)
        {
            _intervenienteAppService.AdicionarHistorico(User.Identity.Name, intervenienteHistorico);

            return CreateResponse(HttpStatusCode.OK, "Historico inserido com sucesso!");
        }

        [HttpPost]
        [Route("v1/interveniente/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> AdicionarAnexo([FromBody] IntervenienteAnexoAdicionarRequest anexo)
        {
            _intervenienteAppService.AdicionarAnexo(User.Identity.Name, anexo, out string mensagem);

            if(mensagem == "")
                return CreateResponse(HttpStatusCode.OK, "Anexo inserido com sucesso!");

            return CreateResponse(HttpStatusCode.BadRequest, mensagem);
        }

        [HttpGet]
        [Route("v1/interveniente/codigo/{intervenienteCodigo}/ano/{anoChamada}/numero/{numeroChamada}/anexos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAnexos(int intervenienteCodigo, int anoChamada, int numeroChamada)
        {
            var anexos = _intervenienteAppService.ListarAnexos(intervenienteCodigo, anoChamada, numeroChamada);

            return CreateResponse(HttpStatusCode.OK, anexos);
        }

        [HttpGet]
        [Route("v1/interveniente/oportunidade/codigo/{intervenienteCodigo}/usina/{usina}/ano/{anoOportunidade}/numero/{numeroOportunidade}/anexos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAnexosPorOportunidade(int intervenienteCodigo, int usina, int anoOportunidade, int numeroOportunidade)
        {
            var anexos = _intervenienteAppService.ListarAnexosPorOportunidade(intervenienteCodigo, usina, anoOportunidade, numeroOportunidade);

            return CreateResponse(HttpStatusCode.OK, anexos);
        }

        [HttpGet]
        [Route("v1/interveniente/codigo/{intervenienteCodigo}/nome-arquivo/{nome}/ano/{anoChamada}/numero/{numeroChamada}/anexo")]
        [Authorize]
        public string ObterAnexo(int intervenienteCodigo, string nome, int anoChamada, int numeroChamada, [FromUri(Name = "data-hora")] DateTime dataHora)
        {
            var anexo = _intervenienteAppService.ObterAnexo(intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada);

            return _intervenienteAppService.ObterAnexoConvertidoBase64(anexo, nome);
        }

        [HttpPatch]
        [Route("v1/interveniente/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> AtualizarDescricaoAnexo([FromBody] IntervenienteAnexoAtualizarRequest anexo)
        {
            _intervenienteAppService.AtualizarDescricaoAnexo(anexo);

            return CreateResponse(HttpStatusCode.OK, "Descrição atualizada com sucesso!");
        }

        [HttpDelete]
        [Route("v1/interveniente/codigo/{intervenienteCodigo}/nome-arquivo/{nome}/ano/{anoChamada}/numero/{numeroChamada}/anexo")]
        [Authorize]
        public Task<HttpResponseMessage> RemoverAnexo(int intervenienteCodigo, string nome, int anoChamada, int numeroChamada, [FromUri(Name = "data-hora")] DateTime dataHora)
        {
            _intervenienteAppService.RemoverAnexo(intervenienteCodigo, nome, dataHora, anoChamada, numeroChamada);

            return CreateResponse(HttpStatusCode.OK, "Anexo deletado com Sucesso");
        }

        //Public Integration

        [HttpPost]
        [Route("integrations/stakeholder")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteAdicionar([FromBody] [Required] IntervenienteRequest request)
        {
            var result = _intervenienteAppService.IntervenienteAdicionar(request.Intervenientes);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/stakeholder/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteAtualizarPorId([FromUri] int code, [FromBody] [Required] IntervenienteAtualizarRequest request)
        {
            var result = _intervenienteAppService.AtualizarPorId(code, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/stakeholder/by-cnpj-cpf/{cnpj_cpf}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteAtualizarPorCnpjCpf([FromUri(Name = "cnpj_cpf")] string cnpjCpf, [FromBody] [Required] IntervenienteAtualizarRequest request)
        {
            var result = _intervenienteAppService.AtualizarPorCnpjCpf(cnpjCpf, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/stakeholder/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteAtualizarPorExternalId([FromUri(Name = "external_id")] string externalId, [FromBody] [Required] IntervenienteAtualizarRequest request)
        {
            var result = _intervenienteAppService.AtualizarPorExternalId(externalId, request);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/stakeholder")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteListar() 
        {
            var queryParameters = HttpContext.Current.Request.QueryString;
            int.TryParse(queryParameters["page"], out int page);
            int.TryParse(queryParameters["limit"], out int limit);

            var result = _intervenienteAppService.Listar(page, limit);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/stakeholder/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteObterPorId([FromUri] int code) 
        {
            var result = _intervenienteAppService.ObterPorId(code); 

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/stakeholder/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteObterPorExternalId([FromUri(Name = "external_id")] string externalId)
        {
            var result = _intervenienteAppService.ObterPorExternalId(externalId); 

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/stakeholder/by-cnpj-cpf/{cnpj_cpf}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteObterPorCnpjCpf([FromUri(Name = "cnpj_cpf")] string cnpjCpf)
        {
            var result = _intervenienteAppService.ObterPorCnpjCpf(cnpjCpf); 

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/stakeholder/by-update-date")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> IntervenienteObterPorDataAtualizacao([FromUri] [Required] DateTime start_date, DateTime? end_date = null, int page = 1, int limit = 10)
        {
            if (start_date.TimeOfDay != TimeSpan.Zero)
                start_date = DateTime.SpecifyKind(start_date, DateTimeKind.Utc).ToLocalTime();

            if (end_date != null && end_date.Value.TimeOfDay != TimeSpan.Zero)
                end_date = DateTime.SpecifyKind(end_date ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();

            var result = _intervenienteAppService.ObterPorDataAtualizacao(start_date, end_date, page, limit);

            return CreateResponse(result);
        }
    }
}