using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.Endereco;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class EnderecoController: BaseController
    {

        private readonly IEnderecoApplicationService _enderecoAppService;

        public EnderecoController(IEnderecoApplicationService enderecoAppService)
        {
            _enderecoAppService = enderecoAppService;
        }

        [HttpGet]
        [Route("v1/endereco/cep/{cep}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorCep(string cep)
        {
            var endereco = _enderecoAppService.ObterPorCep(cep);

            return CreateResponse(HttpStatusCode.OK, endereco);
        }

        [HttpGet]
        [Route("v1/endereco/municipios")]
        [Authorize]
        public Task<HttpResponseMessage> ListarMunicipios()
        {
            var municipios = _enderecoAppService.ListarMunicipios();

            return CreateResponse(HttpStatusCode.OK, municipios);
        }

        [HttpGet]
        [Route("v1/endereco/cep/{cep}/usina/{idUsina}/atende")]
        [Authorize]
        public Task<HttpResponseMessage> UsinaAtendeCep(int idUsina, string cep)
        {
            var atende = _enderecoAppService.UsinaAtendeCep(idUsina, cep);

            return CreateResponse(HttpStatusCode.OK, atende);
        }

        [HttpGet]
        [Route("v1/endereco/cep/{cep}/usina/{idUsina}/valor-adicional")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep)
        {
            var valor = _enderecoAppService.ObterValorAdicionalM3PorUsinaCep(idUsina, cep);

            return CreateResponse(HttpStatusCode.OK, valor);
        }

        [HttpGet]
        [Route("v1/endereco/cep/{cep}/usina/{idUsina}/distancia-km")]
        [Authorize]
        public Task<HttpResponseMessage> ObterDistanciaKmPorUsinaCep(int idUsina, string cep)
        {
            var usinaDistanciaCep = _enderecoAppService.ObterDistanciaKmPorUsinaCep(idUsina, cep);

            return CreateResponse(HttpStatusCode.OK, usinaDistanciaCep?.DistanciaKm ?? 0);
        }

        [HttpGet]
        [Route("v1/endereco/cep/{cep}/usina/{idUsina}/distancia-km-aprovada")]
        [Authorize]
        public Task<HttpResponseMessage> DistanciaKmUsinaCepAprovada(int idUsina, string cep)
        {
            var usinaDistanciaCep = _enderecoAppService.ObterDistanciaKmPorUsinaCep(idUsina, cep);

            return CreateResponse(HttpStatusCode.OK, (usinaDistanciaCep?.IdAprovacao ?? "") != "");
        }

        [HttpGet]
        [Route("v1/endereco/usina/{idUsina}/distancia-km-sugerida-google")]
        [Authorize]
        public Task<HttpResponseMessage> DistanciaKmUsinaGoogleSugerida(int idUsina, [FromUri] string destino)
        {
            var distancia = _enderecoAppService.ObterDistanciaEntreUsinaEObraViaGoogleMatrixApi(idUsina, destino);
            
            return CreateResponse(HttpStatusCode.OK, distancia);
        }
    }
}