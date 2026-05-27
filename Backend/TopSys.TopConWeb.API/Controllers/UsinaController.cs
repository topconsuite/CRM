using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.Usina;
using TopSys.TopConWeb.Application.Interfaces;


namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class UsinaController : BaseController
    {
        private readonly IUsinaApplicationService _usinaApplicationService;

        public UsinaController(IUsinaApplicationService usinaApplicationService)
        {
            _usinaApplicationService = usinaApplicationService;
        }

        [HttpGet]
        [Route("v1/usinas")]
        //[Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaUsinas = _usinaApplicationService.ListarUsinasAtivas();

            return CreateResponse(HttpStatusCode.OK, listaUsinas);
        }

        [HttpGet]
        [Route("v1/usinas-por-empresa/{empresa}")]
        //[Authorize]
        public Task<HttpResponseMessage> ListarPorEmpresa(int empresa)
        {
            var listaUsinas = _usinaApplicationService.ListarPorEmpresa(empresa);

            return CreateResponse(HttpStatusCode.OK, listaUsinas);
        }

        [HttpGet]
        [Route("v1/usina/{idUsina}/parametro-programacao")]
        [Authorize]
        public Task<HttpResponseMessage> ObterParametroProgramacao(int idUsina)
        {
            var parametroProgramacao = _usinaApplicationService.ObterParametroProgramacao(idUsina);

            return CreateResponse(HttpStatusCode.OK, parametroProgramacao);
        }

        [HttpGet]
        [Route("v1/usina/{idUsina}/distancia-km/{km}/atende")]
        [Authorize]
        public Task<HttpResponseMessage> UsinaAtendeKm(int idUsina, int km)
        {
            var atende = _usinaApplicationService.UsinaAtendeKm(idUsina, km);

            return CreateResponse(HttpStatusCode.OK, atende);
        }

        [HttpGet]
        [Route("v1/usina/{idUsina}/distancia-km/{km}/valor-adicional")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorAdicionalM3PorUsinaKm(int idUsina, int km)
        {
            var valor = _usinaApplicationService.ObterValorAdicionalM3PorUsinaKm(idUsina, km);

            return CreateResponse(HttpStatusCode.OK, valor);
        }

        [HttpGet]
        [Route("v1/usinas-permitidas")]
        [Authorize]
        public Task<HttpResponseMessage> ListarUsinasPermitidasPorUsuario()
        {
            var listaUsinas = _usinaApplicationService.ListarUsinasPermitidasUsuario(User.Identity.Name);

            return CreateResponse(HttpStatusCode.OK, listaUsinas);
        }
    }
}