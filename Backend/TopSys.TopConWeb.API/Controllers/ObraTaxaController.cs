using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.CadastroDiverso;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class ObraTaxaController : BaseController
    {
        private readonly IObraTaxaApplicationService _obraTaxaAppService;
        private readonly ICadastroDiversoApplicationService _cadastroDiversoAppService;

        public ObraTaxaController(
            IObraTaxaApplicationService obraTaxaAppService,
            ICadastroDiversoApplicationService cadastroDiversoAppService)
        {
            _obraTaxaAppService = obraTaxaAppService;
            _cadastroDiversoAppService = cadastroDiversoAppService;
        }

        [HttpGet]
        [Route("v1/obra-taxa/opcoes/{campo}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarOpcoes(string campo)
        {
            var opcoes = _cadastroDiversoAppService
                .ListarFiltrados<CadastroDiversoResponse>(t =>
                    t.Aplicativo == "topcon" && t.ProgramaNumero == 6022
                    && t.ProgramaCampo == campo
                );

            return CreateResponse(HttpStatusCode.OK, opcoes);
        }

        [HttpGet]
        [Route("v1/obra-taxas/usina/{idUsina}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTaxaPadraoByIdUsina(int idUsina)
        {
            var taxas = _obraTaxaAppService.ListarTaxaPadraoByIdUsina(idUsina);

            return CreateResponse(HttpStatusCode.OK, taxas);
        }

        [HttpGet]
        [Route("v1/obra-taxas/usina/{idUsina}/segmento/{idSegmentacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTaxaPadraoByIdUsinaSegmento(int idUsina, int idSegmentacao)
        {
            var taxas = _obraTaxaAppService.ListarTaxaPadraoByIdUsinaSegmento(idUsina, idSegmentacao);

            return CreateResponse(HttpStatusCode.OK, taxas);
        }
    }
}