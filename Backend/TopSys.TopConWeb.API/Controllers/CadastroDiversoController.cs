using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CadastroDiversoController : BaseController
    {
        private readonly ICadastroDiversoApplicationService _cadastroDiversoAppService;

        public CadastroDiversoController(ICadastroDiversoApplicationService cadastroDiversoAppService)
        {
            _cadastroDiversoAppService = cadastroDiversoAppService;
        }

        [HttpGet]
        [Route("v1/cadastro-diverso/andares")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAndares()
        {
            var andares = _cadastroDiversoAppService.ListarAndares();

            return CreateResponse(HttpStatusCode.OK, andares);
        }


        [HttpGet]
        [Route("v1/cadastro-diverso/condicoes-pagamento")]
        [Authorize]
        public Task<HttpResponseMessage> ListarCondicoesPagamento()
        {
            var condicoes = _cadastroDiversoAppService.ListarCondicoesPagamento();

            return CreateResponse(HttpStatusCode.OK, condicoes);
        }

        [HttpGet]
        [Route("v1/cadastro-diverso/dias-semana-fixo")]
        [Authorize]
        public Task<HttpResponseMessage> ListarDiasDaSemanaFixo()
        {
            var dias = _cadastroDiversoAppService.ListarDiasDaSemanaFixo();

            return CreateResponse(HttpStatusCode.OK, dias);
        }

        [HttpGet]
        [Route("v1/cadastro-diverso/opcoes-vencimento-dia-nao-util")]
        [Authorize]
        public Task<HttpResponseMessage> ListarOpcoesDeVencimentoEmDiaNaoUtil()
        {
            var opcoes = _cadastroDiversoAppService.ListarOpcoesDeVencimentoEmDiaNaoUtil();

            return CreateResponse(HttpStatusCode.OK, opcoes);
        }

        [HttpGet]
        [Route("v1/cadastro-diverso/quantidade-de-corpos-de-prova")]
        [Authorize]
        public Task<HttpResponseMessage> ListarQuantidadeDeCorposDeProva()
        {
            var opcoes = _cadastroDiversoAppService.ListarQuantidadeDeCorposDeProva();

            return CreateResponse(HttpStatusCode.OK, opcoes);
        }


        [HttpGet]
        [Route("v1/cadastro-diverso/modelo-documento-remessa-concreto")]
        [Authorize]
        public Task<HttpResponseMessage> ListarModeloDocumentoRemessaConcreto()
        {
            var opcoes = _cadastroDiversoAppService.ListarModeloDocumentoRemessaConcreto();

            return CreateResponse(HttpStatusCode.OK, opcoes);
        }
    }
}