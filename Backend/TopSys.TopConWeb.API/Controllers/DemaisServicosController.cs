using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Inclusao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class DemaisServicosController: BaseController
    {
        private readonly IDemaisServicosApplicationService _demaisServicosApplicationService;

        public DemaisServicosController(IDemaisServicosApplicationService demaisServicosApplicationService)
        {
            _demaisServicosApplicationService = demaisServicosApplicationService;
        }

        [HttpPost]
        [Route("v1/demais-servicos")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] DemaisServicosInclusaoRequest request)
        {
            _demaisServicosApplicationService.Adicionar(request);

            return CreateResponse(HttpStatusCode.OK, "Serviço Adicionado com Sucesso");
        }

        [HttpPatch]
        [Route("v1/demais-servicos")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] DemaisServicosAlteracaoRequest request)
        {
            _demaisServicosApplicationService.Atualizar(request); 

            return CreateResponse(HttpStatusCode.OK, "Serviço Alterado com Sucesso");
        }

        [HttpDelete]
        [Route("v1/demais-servicos/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _demaisServicosApplicationService.Deletar(codigo);

            return CreateResponse(HttpStatusCode.OK, "Serviço deletado com Sucesso");
        }

        [HttpGet]
        [Route("v1/demais-servicos")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] DemaisServicosPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<DemaisServicos>(request.Filter);

            var pagedList = _demaisServicosApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/demais-servicos/usina/{usinaId}/servico/{servicoId}")]
        [Authorize]
        public Task<HttpResponseMessage> Obter(int usinaId, int servicoId)
        {
            var pagedList = _demaisServicosApplicationService.Listar(1, 10, 
                t => t.Codigo == servicoId && t.UsinaCodigo == usinaId );

            return CreateResponse(HttpStatusCode.OK, pagedList);
        }

    }
}