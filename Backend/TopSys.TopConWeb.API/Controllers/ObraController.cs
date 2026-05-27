using LinqKit;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.Obra;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ConsularObrasRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarDadosFiscaisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarStatusCadastroEAnalistaRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraBomba;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraDistanciaUsinaCepAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraEngenhariaAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPagamentosAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraPendenteAprovacaoRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecao;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraTraco;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraZMRCAprovacaoRequest;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Infra.Legacy.Filters;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ObraController : BaseController
    {

        private readonly IObraApplicationService _obraAppService;
        private readonly IObraTaxaApplicationService _obraTaxaAppService;
        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IAprovacaoComercialApplicationService _aprovacaoComercialApplicationService;
        private readonly ILiberacaoAcessoApplicationService _liberacaoAcessoApplicationService;
        private readonly IParametroApplicationService _parametroService;

        public ObraController(
            IObraApplicationService obraService, 
            IObraTaxaApplicationService obraTaxaAppService, 
            IComercialLegacyService comercialLegacyService,
            IAprovacaoComercialApplicationService aprovacaoComercialApplicationService,
            ILiberacaoAcessoApplicationService liberacaoAcessoApplicationService,
            IParametroApplicationService parametroService) 
        {
            _obraAppService = obraService;
            _obraTaxaAppService = obraTaxaAppService;
            _comercialLegacyService = comercialLegacyService;
            _aprovacaoComercialApplicationService = aprovacaoComercialApplicationService;
            _liberacaoAcessoApplicationService = liberacaoAcessoApplicationService;
            _parametroService = parametroService;
        }

        [HttpGet]
        [Route("v1/obra/pendente")]
        [Authorize]
        public Task<HttpResponseMessage> ListaPendentesDeAprovacao()
        {
            var obrasPendentes = _obraAppService.ListaPendentesDeAprovacao(User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, obrasPendentes);
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/numero/{numero}/ano-chamada/{anoChamada}/numero-chamada/{noChamada}/tem-aprovacao-pendente")]
        [Authorize]
        public Task<HttpResponseMessage> TemAprovacaoPendente(int usina, int numero, int anoChamada, int noChamada)
        {
            var temPendenciaAprovacao = _obraAppService.TemAprovacaoPendente(usina, numero, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, temPendenciaAprovacao);
        }

        [HttpGet]
        [Route("v1/obra/pendente/{usina},{numero},{anoChamada},{noChamada},{numeroContrato},{anoContrato}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPendentePorId(int usina, int numero, int anoChamada, int noChamada, int numeroContrato, int anoContrato)
        {
            var obra = _obraAppService.ObtemPendentePorId(usina, numero, anoChamada, noChamada, numeroContrato, anoContrato, User.Identity.Name);
            
            return CreateResponse(HttpStatusCode.OK, obra);
        }

        [HttpPost]
        [Route("v1/obra/pendente")]
        [Route("v1/obra/pendente/aprovar")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarObraPendente(ObraPendenteAprovacaoRequest obra)
        {
            var liberacaoAcesso = _liberacaoAcessoApplicationService.ObterLiberacaoAcessoUsuario(User.Identity.Name);
            if (!liberacaoAcesso)
                return CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");

            //_obraAppService.AprovarObraPendente(User.Identity.Name, obra);
            var log = new StringBuilder();

            _aprovacaoComercialApplicationService.AprovarObraPendente(User.Identity.Name, obra);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/numero/{numero}/ano-chamada/{anoChamada}/numero-chamada/{noChamada}/logs")]
        [Authorize]
        public Task<HttpResponseMessage> ListarObraLogsPorId(int usina, int numero, int anoChamada, int noChamada)
        {
            var obraLogs = _obraAppService.ListarObraLogsPorId(usina, numero, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, obraLogs);
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/numero/{numero}/ano-chamada/{anoChamada}/numero-chamada/{noChamada}/ano-contrato/{anoContrato}/numero-contrato/{numContrato}/logs")]
        [Authorize]
        public Task<HttpResponseMessage> ListarObraLogsPorId(int usina, int numero, int anoChamada, int noChamada, int anoContrato, int numContrato)
        {
            var obraLogs = _obraAppService.ListarObraLogsPorId(usina, numero, anoChamada, noChamada, anoContrato, numContrato);

            return CreateResponse(HttpStatusCode.OK, obraLogs);
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/numero/{numero}/taxas")]
        [Authorize]
        public Task<HttpResponseMessage> ListarObraTaxasPorId(int usina, int numero)
        {
            var taxas = _obraTaxaAppService.ListarByIdObra(usina, numero);

            return CreateResponse(HttpStatusCode.OK, taxas);
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/taxas-padrao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTaxasPadraoByIdUsina(int usina)
        {
            var taxas = _obraTaxaAppService.ListarTaxaPadraoByIdUsina(usina);

            return CreateResponse(HttpStatusCode.OK, taxas);
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/segmento/{idSegmentacao}/taxas-padrao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTaxasPadraoByIdUsinaAndSegmento(int usina, int idSegmentacao)
        {

            var taxas = _obraTaxaAppService.ListarTaxaPadraoByIdUsinaSegmento(usina, idSegmentacao);

            return CreateResponse(HttpStatusCode.OK, taxas);

        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/numero/{numero}/tem-bomba/{temBomba}/volume-total/{volumeTotal}/volume-por-carga/{volumePorCarga}/valor-m3-faltante")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorM3Faltante(int usina, int numero, bool temBomba, float volumeTotal, float volumePorCarga)
        {
            var valor = _obraTaxaAppService.ObterValorM3Faltante(temBomba, volumeTotal, volumePorCarga, usina, numero);

            return CreateResponse(HttpStatusCode.OK, valor);
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/tem-bomba/{temBomba}/volume-total/{volumeTotal}/volume-por-carga/{volumePorCarga}/valor-m3-faltante")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorM3Faltante(int usina, bool temBomba, float volumeTotal, float volumePorCarga)
        {
            var valor = _obraTaxaAppService.ObterValorM3Faltante(temBomba, volumeTotal, volumePorCarga, usina);

            return CreateResponse(HttpStatusCode.OK, valor);
        }

        [HttpGet]
        [Route("v1/obras/usina/{idUsina}/obra-numero/{obraNumero}/numero-cartao/{numeroCartao}/autorizacao/{autorizacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int obraNumero, int numeroCartao, string autorizacao)
        {
            var obras = _obraAppService.ListarPorNumeroCartaoAutorizacaoDuplicado(idUsina, obraNumero, numeroCartao, autorizacao);

            return CreateResponse(HttpStatusCode.OK, obras);
        }

        [HttpGet]
        [Route("v1/obras/projecao/listar")]
        [Authorize]
        public Task<HttpResponseMessage> ConsultarObrasPorCarteira([FromUri] ObraProjecaoPagedRequest request)
        {

            var filtroVendedores = User.Identity.VendedoresPermitidos();
            var urlFilter = UrlFilterParser.Parse<Obra>(request.Filter);

            if(!filtroVendedores.Equals("*"))
            {
                var vendedores = filtroVendedores.Split(',');
                urlFilter = urlFilter.And(x => vendedores.Contains(x.Contrato.VendedorCodigo.ToString()));
            }
            
            var response = _obraAppService.ListarObraPorPaginaParaCarteira(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, response);    

        }

        [HttpGet]
        [Route("v1/obras/consultar")]
        [Authorize]
        public Task<HttpResponseMessage> ConsultarObras([FromUri] ConsultarObrasRequest request)
        {
            var filtroVendedores = User.Identity.VendedoresPermitidos();

            var filtro = UrlFilterParser.Convert<ConsultarObraFilter>(request.Filter);
            filtro.VendedoresPermitidos = filtroVendedores;

            if (request.Ordenacao == null) request.Ordenacao = "";

            var ordenacao = RetornaOrdenacao(request.Ordenacao);

            var resultado = _comercialLegacyService.ConsultarObras(filtro, request.Pagina, request.PorPagina, User.Identity.Name, ordenacao);

            return CreatePagedResponse(HttpStatusCode.OK, resultado);
        }

        [HttpGet]
        [Route("v1/obra/usina/{idUsina}/numero/{obraNumero}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}/pagamentos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarObraPagamentos(int idUsina, int obraNumero, int numeroContrato, int anoContrato)
        {
            var response = _obraAppService.ListarObraPagamentos(idUsina, obraNumero, numeroContrato, anoContrato);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("v1/obra/aprovar-pagamentos")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarObraPagamentos(ObraPagamentosAprovacaoRequest obra)
        {
            var liberacaoAcesso = _liberacaoAcessoApplicationService.ObterLiberacaoAcessoUsuario(User.Identity.Name);
            if (!liberacaoAcesso)
                return CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");

            _obraAppService.AprovarObraPagamentos(User.Identity.Name, obra);

            return CreateResponse(HttpStatusCode.OK, "");

        }

        [HttpGet]
        [Route("v1/obra/usina/{idUsina}/numero/{obraNumero}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}/tracos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarObraTracos(int idUsina, int obraNumero, int numeroContrato, int anoContrato)
        {
            var response = _obraAppService.ListarObraTracos(idUsina, obraNumero, numeroContrato, anoContrato);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("v1/obra/aprovar-engenharia")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarEngenharia(ObraEngenhariaAprovacaoRequest obra)
        {
            var liberacaoAcesso = _liberacaoAcessoApplicationService.ObterLiberacaoAcessoUsuario(User.Identity.Name);
            if (!liberacaoAcesso)
                return CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");

            _obraAppService.AprovarEngenharia(User.Identity.Name, obra);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/obra/usina/{idUsina}/numero/{obraNumero}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}/analise/cadastro")]
        [Authorize]
        public Task<HttpResponseMessage> ObterObraParaAnaliseCadastro(int idUsina, int obraNumero, int numeroContrato, int anoContrato)
        {
            var response = _obraAppService.ObterObraParaAnaliseCadastro(idUsina, obraNumero, numeroContrato, anoContrato);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/obra/tempo-descarga/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterTempoDescarga(int idUsina)
        {
            return CreateResponse(HttpStatusCode.OK, _obraAppService.ObterTempoDescarga(idUsina).TempoDescarga);
        }

        [HttpPost]
        [Route("v1/obra/alterar-dados-fiscais")]
        [Authorize]
        public Task<HttpResponseMessage> AlterarDadosFiscais(ObraAlterarDadosFiscaisRequest obra)
        {
            _obraAppService.AlterarDadosFiscais(obra, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("v1/obra/aprovar-distancia-usina-cep")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarDistanciaUsinaCep(ObraDistanciaUsinaCepAprovacaoRequest obra)
        {
            _obraAppService.AprovarDistanciaUsinaCep(User.Identity.Name, obra);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("v1/obra/alterar-status-cadastro-e-analista")]
        [Authorize]
        public Task<HttpResponseMessage> AlterarStatusCadastroEAnalista(ObraAlterarStatusCadastroEAnalistaRequest obra)
        {
            var liberacaoAcesso = _liberacaoAcessoApplicationService.ObterLiberacaoAcessoUsuario(User.Identity.Name);
            if (!liberacaoAcesso)
                return CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            
            _obraAppService.AlterarStatusCadastroEAnalista(obra, User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("v1/obra/aprovacao-cielo-lio/usina/{idUsina}/ano-chamada/{ano}/numero-chamada/{numero}/")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarAutomaticamentePagamentosDaCieloLio(int idUsina, int ano, int numero)
        {
            _obraAppService.AprovarAutomaticamentePagamentosDaCieloLio(User.Identity.Name, idUsina, ano, numero);

            return CreateResponse(HttpStatusCode.OK, "");

        }

        [HttpPost]
        [Route("v1/obra/calcular-ebitda-traco")]
        [Authorize]
        public Task<HttpResponseMessage> CalcularEbitdaObraTraco(CalcularEbitdaObraTraco obraTraco)
        {
            var response = _obraAppService.CalcularEbitdaObraTraco(obraTraco);

            return CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpPost]
        [Route("v1/obra/calcular-ebitda-bomba")]
        [Authorize]
        public Task<HttpResponseMessage> CalcularEbitdaObraBomba(CalcularEbitdaObraBomba obraBomba)
        {
            var response = _obraAppService.CalcularEbitdaObraBomba(obraBomba);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/obra/consumo-traco/usinaContrato/{usinaContrato}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}/sequenciaTraco/{sequenciaTraco}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterConsumoTracoPorContrato(int usinaContrato, int numeroContrato, int anoContrato, int sequenciaTraco)
        {
       
            var response = _obraAppService.ObterConsumoTracoPorContrato(usinaContrato, numeroContrato, anoContrato, sequenciaTraco);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/obra/consumo-por-traco/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}/traco-resistencia/{tracoResistencia}/traco-pedra/{tracoPedra}/traco-slump-codigo/{slumpCodigo}/uso/{tracoUso}/traco-slump-variacao/{slumpVariacao}/interveniente/{interveniente}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterConsumoPorTraco(int numeroContrato, int AnoContrato, string tracoResistencia, int tracoPedra, int slumpCodigo, int tracoUso, int slumpVariacao, int interveniente)
        {
            var response = _obraAppService.ObterConsumoPorTraco(numeroContrato, AnoContrato, tracoResistencia, tracoPedra, slumpCodigo, tracoUso, slumpVariacao, interveniente);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("v1/obra/aprovar-ZMRC")]
        [Authorize]
        public Task<HttpResponseMessage> AprovarZMRC(ObraZMRCAprovacaoRequest obra)
        {
            _obraAppService.AprovarZmrc(User.Identity.Name, obra);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpPost]
        [Route("v1/obra/reprovar-ZMRC")]
        [Authorize]
        public Task<HttpResponseMessage> ReprovarZMRC(ObraZMRCAprovacaoRequest obra)
        {
            _obraAppService.ReprovarZmrc(User.Identity.Name, obra);

            return CreateResponse(HttpStatusCode.OK, "");
        }

        [HttpGet]
        [Route("v1/obra/usina/{usina}/numero/{numero}/ano-chamada/{anoChamada}/numero-chamada/{noChamada}/projecao")]
        [Authorize]
        public Task<HttpResponseMessage> ListarProjecaoPorObra(int usina, int numero, int anoChamada, int noChamada)
        {
            var obraProjecao = _obraAppService.ListarProjecaoPorObra(usina, numero, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, obraProjecao);
        }

        [HttpGet]
        [Route("v1/obra/consumo-contrato/usinaContrato/{usinaContrato}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterConsumoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {

            var response = _obraAppService.ObterConsumoPorContrato(usinaContrato, numeroContrato, anoContrato);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/obra/volume-contrato/usina/{usina}/noObra/{noObra}/anoChamada/{anoChamada}/noChamada/{noChamada}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterVolumePorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {

            var response = _obraAppService.ObterVolumePorContrato(usina, noObra, anoChamada, noChamada);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/obra/consumo-acumulado-contrato/usinaContrato/{usinaContrato}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterConsumoAcumuladoPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {

            var response = _obraAppService.ObterConsumoAcumuladoPorContrato(usinaContrato, numeroContrato, anoContrato);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("v1/obra/consumo-mes-contrato/usinaContrato/{usinaContrato}/numeroContrato/{numeroContrato}/anoContrato/{anoContrato}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterConsumoMesAtualPorContrato(int usinaContrato, int numeroContrato, int anoContrato)
        {

            var response = _obraAppService.ObterConsumoMesAtualPorContrato(usinaContrato, numeroContrato, anoContrato);

            return CreateResponse(HttpStatusCode.OK, response);
        }

        private string RetornaOrdenacao(string ordenacao)
        {
            if (ordenacao.Contains("interveniente"))
                ordenacao = ordenacao.Replace("interveniente", "b.ClienteCodigo");

            if (ordenacao.Contains("contrato ASC"))
                ordenacao = ordenacao.Replace("contrato ASC", "b.Usina ASC, b.ContratoAno ASC, b.ContratoNumero ASC");
            else if (ordenacao.Contains("contrato DESC"))
                ordenacao = ordenacao.Replace("contrato DESC", "b.Usina DESC, b.ContratoAno DESC, b.ContratoNumero DESC");

            if (ordenacao.Contains("clienteCpfCnpj"))
                ordenacao = ordenacao.Replace("clienteCpfCnpj", "b.ClienteCpfCnpj");

            if (ordenacao.Contains("obra "))
                ordenacao = ordenacao.Replace("obra ", "b.ObraNome ");

            if (ordenacao.Contains("vendedor"))
                ordenacao = ordenacao.Replace("vendedor", "b.VendedorCodigo");

            if (ordenacao.Contains("dataHoraProgramacao ASC"))
                ordenacao = ordenacao.Replace("dataHoraProgramacao ASC", "b.DataConcretagem ASC, b.Horario ASC");
            else if(ordenacao.Contains("dataHoraProgramacao DESC"))
                ordenacao = ordenacao.Replace("dataHoraProgramacao DESC", "b.DataConcretagem DESC, b.Horario DESC");

            if (ordenacao.Contains("usinaEntrega"))
                ordenacao = ordenacao.Replace("usinaEntrega", "b.UsinaPrincipal");

            if (ordenacao.Contains("tipoCobranca"))
                ordenacao = ordenacao.Replace("tipoCobranca", "b.TipoCobrancaCodigo");

            if (ordenacao.Contains("obraMunicipio"))
                ordenacao = ordenacao.Replace("obraMunicipio", "b.ObraMunicipio");

            if (ordenacao.Contains("proposta ASC"))
                ordenacao = ordenacao.Replace("proposta ASC", "b.Usina ASC, b.PropostaAno ASC, b.PropostaNumero ASC");
            else if (ordenacao.Contains("proposta DESC"))
                ordenacao = ordenacao.Replace("proposta DESC", "b.Usina DESC, b.PropostaAno DESC, b.PropostaNumero DESC");

            if (ordenacao.Contains("obraContato"))
                ordenacao = ordenacao.Replace("obraContato", "b.ObraContato");

            if (ordenacao.Contains("telefone ASC"))
                ordenacao = ordenacao.Replace("telefone ASC", "b.ClienteTelefoneDdd ASC, b.ClienteTelefoneNumero ASC");
            else if (ordenacao.Contains("telefone DESC"))
                ordenacao = ordenacao.Replace("telefone DESC", "b.ClienteTelefoneDdd DESC, b.ClienteTelefoneNumero DESC");

            if (ordenacao.Contains("celular ASC"))
                ordenacao = ordenacao.Replace("celular ASC", "b.ClienteCelularDdd ASC, b.ClienteCelularNumero ASC");
            else if (ordenacao.Contains("celular DESC"))
                ordenacao = ordenacao.Replace("celular DESC", "b.ClienteCelularDdd DESC, b.ClienteCelularNumero DESC");

            if (ordenacao.Contains("telefoneComercial ASC"))
                ordenacao = ordenacao.Replace("telefoneComercial ASC", "b.ClienteTelefoneComercialDdd ASC, b.ClienteTelefoneComercialNumero ASC");
            else if (ordenacao.Contains("telefoneComercial DESC"))
                ordenacao = ordenacao.Replace("telefoneComercial DESC", "b.ClienteTelefoneComercialDdd DESC, b.ClienteTelefoneComercialNumero DESC");

            if (ordenacao.Contains("contratoData"))
                ordenacao = ordenacao.Replace("contratoData", "b.ContratoData");

            if (ordenacao.Contains("analista"))
                ordenacao = ordenacao.Replace("analista", "b.AnalistaCodigo");

            if (ordenacao.Contains("volumeTotal"))
                ordenacao = ordenacao.Replace("volumeTotal", "b.VolumeTotal");

            if (ordenacao.Contains("contratoValorTotal"))
                ordenacao = ordenacao.Replace("contratoValorTotal", "b.ContratoValorTotal");

            if (ordenacao.Contains("grupoEconomico"))
                ordenacao = ordenacao.Replace("grupoEconomico", "b.GrupoEconomicoCodigo");

            var ordenacaoFormatada = !ordenacao.Equals("") ? ordenacao.Substring(0, ordenacao.Length - 1) : "";

            return ordenacaoFormatada;
        }
    }
}
