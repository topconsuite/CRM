using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Request.ContratoReajuste;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ContratoReajusteController : BaseController
    {

        private readonly IContratoTracoReajusteApplicationService _contratoTracoReajusteApplicationService;
        private readonly IContratoBombaReajusteApplicationService _contratoBombaReajusteApplicationService;

        public ContratoReajusteController(IContratoTracoReajusteApplicationService contratoTracoReajusteApplicationService, IContratoBombaReajusteApplicationService contratoBombaReajusteApplicationService)
        {
            _contratoTracoReajusteApplicationService = contratoTracoReajusteApplicationService;
            _contratoBombaReajusteApplicationService = contratoBombaReajusteApplicationService;
        }

        [HttpGet]
        [Route("v1/contrato-reajuste/traco/vigencias")]
        [Authorize]
        public Task<HttpResponseMessage> ListarVigenciasTraco()
        {
            var listaVigencias = _contratoTracoReajusteApplicationService.ObterVigencias();

            return CreateResponse(HttpStatusCode.OK, listaVigencias);
        }

        [HttpGet]
        [Route("v1/contrato-reajuste/bomba/vigencias")]
        [Authorize]
        public Task<HttpResponseMessage> ListarVigenciasBomba()
        {
            var listaVigencias = _contratoBombaReajusteApplicationService.ObterVigencias();

            return CreateResponse(HttpStatusCode.OK, listaVigencias);
        }

        [HttpGet]
        [Route("v1/contrato-reajuste/traco")]
        [Authorize]
        public Task<HttpResponseMessage> ListaTodosReajustesTracoPorPagina([FromUri] ContratoReajustePagedRequest request)
        {
            if (request.Filter != null)
                request.Filter = RetornaFiltroFormatado(request.Filter);

            var listaReajustesTraco = _contratoTracoReajusteApplicationService.ListarContratoReajusteTracoPorPagina(request.Pagina, request.PorPagina, request.Filter);

            return CreatePagedResponse(HttpStatusCode.OK, listaReajustesTraco);
        }

        [HttpGet]
        [Route("v1/contrato-reajuste/bomba")]
        [Authorize]
        public Task<HttpResponseMessage> ListaTodosReajustesBombaPorPagina([FromUri] ContratoReajustePagedRequest request)
        {
            if (request.Filter != null)
                request.Filter = RetornaFiltroFormatado(request.Filter);

            var listaReajustesBomba = _contratoBombaReajusteApplicationService.ListarContratoReajusteBombaPorPagina(request.Pagina, request.PorPagina, request.Filter);

            return CreatePagedResponse(HttpStatusCode.OK, listaReajustesBomba);
        }

        [HttpGet]
        [Route("v1/contrato-reajuste/usina/{idUsina}/ano/{ano}/numero/{numero}/dataVigencia/{dataVigencia}/tipo/{tipo}/logs")]
        //[Authorize]
        public Task<HttpResponseMessage> ListarReajusteLogs(int idUsina, int ano, int numero, DateTime dataVigencia, string tipo)
        {
            IEnumerable<ContratoReajusteLogResponse> reajusteLogs = new List<ContratoReajusteLogResponse>();
            if (tipo.Equals("traco"))
                reajusteLogs = _contratoTracoReajusteApplicationService.ListaReajusteLogs(idUsina, ano, numero, dataVigencia);
            else
                reajusteLogs = _contratoBombaReajusteApplicationService.ListaReajusteLogs(idUsina, ano, numero, dataVigencia);

            return CreateResponse(HttpStatusCode.OK, reajusteLogs);
        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/aprovar/traco")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarReajusteTraco([FromBody] ContratoReajusteAlteracaoRequest contratoReajusteAlteracao)
        {

            _contratoTracoReajusteApplicationService.AprovarReajuste(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajuste aprovado com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/aprovar-todos/traco")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarTodosReajusteTraco([FromBody] IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajusteAlteracao)
        {

            _contratoTracoReajusteApplicationService.AprovarTodos(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajustes aprovados com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/reprovar/traco")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarReajusteTraco([FromBody] ContratoReajusteAlteracaoRequest contratoReajusteAlteracao)
        {

            _contratoTracoReajusteApplicationService.ReprovarReajuste(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajuste reprovado com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/reprovar-todos/traco")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarTodosReajusteTraco([FromBody] IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajusteAlteracao)
        {

            _contratoTracoReajusteApplicationService.ReprovarTodos(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajustes reprovados com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/aprovar/bomba")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarReajusteBomba([FromBody] ContratoReajusteAlteracaoRequest contratoReajusteAlteracao)
        {

            _contratoBombaReajusteApplicationService.AprovarReajuste(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajuste aprovado com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/aprovar-todos/bomba")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarTodosReajusteBomba([FromBody] IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajusteAlteracao)
        {

            _contratoBombaReajusteApplicationService.AprovarTodos(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajustes aprovados com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/reprovar/bomba")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarReajusteBomba([FromBody] ContratoReajusteAlteracaoRequest contratoReajusteAlteracao)
        {

            _contratoBombaReajusteApplicationService.ReprovarReajuste(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajuste reprovado com sucesso.");

        }

        [HttpPatch]
        [Route("v1/contrato-reajuste/reprovar-todos/bomba")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarTodosReajusteBomba([FromBody] IEnumerable<ContratoReajusteAlteracaoRequest> contratoReajusteAlteracao)
        {

            _contratoBombaReajusteApplicationService.ReprovarTodos(contratoReajusteAlteracao, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "Reajustes reprovados com sucesso.");

        }

        private string RetornaFiltroFormatado(string filter)
        {
            var pessoaFisica = false;
            var pessoaJuridica = false;
            var construtora = false;

            var filterFormatado = "";
            var filterTipoPessoa = "";

            filter = filter.Replace("$(", "");
            filter = filter.Replace(")", "");

            if (filter.Contains("pessoaFisica"))
            {
                pessoaFisica = true;
                filter = filter.Replace("pessoaFisica==true", "");
            }

            if (filter.Contains("pessoaJuridica"))
            {
                pessoaJuridica = true;
                filter = filter.Replace("pessoaJuridica==true", "");
            }

            if (filter.Contains("construtora"))
            {
                construtora = true;
                filter = filter.Replace("construtora==true", "");
            }

            var filterSplit = filter.Split(';');
            foreach (var item in filterSplit)
            {
                if (!item.Equals(""))
                {
                    if (!filterFormatado.Equals(""))
                        filterFormatado += " AND ";

                    if (item.Contains("inicioValidade"))
                    {
                        filterFormatado += item.Replace("T00:00:00", "'");
                        filterFormatado = filterFormatado.Replace("inicioValidade==", "reaj.dt_vigencia='");
                    }

                    if (item.Contains("contrato.interveniente"))
                        filterFormatado += item.Replace("contrato.interveniente.codigo=", "interv.cod");

                    if (item.Contains("contratoAno"))
                        filterFormatado += item.Replace("contratoAno=", "reaj.ano_contrato");

                    if (item.Contains("contratoNumero"))
                        filterFormatado += item.Replace("contratoNumero=", "reaj.num_contrato");

                    if (item.Contains("status"))
                    {
                        if (item.Equals("status==1"))
                            filterFormatado += item.Replace("status==1", "NOT ISNULL(reaj.dt_confirmacao)");
                        else if (item.Equals("status==2"))
                            filterFormatado += item.Replace("status==2", "(ISNULL(reaj.dt_confirmacao) AND reaj.id_reprovacao='')");
                        else
                            filterFormatado += item.Replace("status==3", "reaj.id_reprovacao<>''");
                    }

                    if (item.Contains("contrato.vendedorCodigo"))
                        filterFormatado += item.Replace("contrato.vendedorCodigo=", "cont.vendedor");

                    if (item.Contains("contrato.usinaPrincipal"))
                        filterFormatado += item.Replace("contrato.usinaPrincipal=", "cont.usina_principal");
                }
            }

            if (pessoaFisica || pessoaJuridica || construtora)
            {
                filterTipoPessoa = "(";

                if (pessoaFisica)
                    filterTipoPessoa += "interv.tp_cliente = 'F'";

                if (pessoaJuridica)
                {
                    if (pessoaFisica)
                        filterTipoPessoa += " OR ";

                    filterTipoPessoa += "interv.tp_cliente='J'";
                }

                if (construtora)
                {
                    if (pessoaFisica || pessoaJuridica)
                        filterTipoPessoa += " OR ";

                    filterTipoPessoa += "interv.tp_cliente='C'";
                }

                filterTipoPessoa += ")";
            }

            filterFormatado += !filterFormatado.Equals("") && !filterTipoPessoa.Equals("") ? " AND " : "";
            filterFormatado += filterTipoPessoa;

            return filterFormatado;
        }
    }
}