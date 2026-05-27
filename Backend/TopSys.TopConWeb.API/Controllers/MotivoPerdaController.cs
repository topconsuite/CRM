using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Application.DTOS.Request.MotivoPerda;
using TopSys.TopConWeb.Application.DTOS.Response.MotivoPerda;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class MotivoPerdaController : BaseController
    {
        private readonly IMotivoPerdaApplicationService _motivoPerdaApplicationService;

        public MotivoPerdaController(IMotivoPerdaApplicationService motivoPerdaApplicationService)
        {
            _motivoPerdaApplicationService = motivoPerdaApplicationService;
        }

        [HttpPost]
        [Route("v1/motivo-perda")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] MotivoPerdaInclusaoRequest motivoPerda)
        {
            _motivoPerdaApplicationService.Adicionar(motivoPerda, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Motivo da Perda adicionado com sucesso");
        }

        [HttpPatch]
        [Route("v1/motivo-perda")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] MotivoPerdaAlteracaoRequest motivoPerda)
        {
            _motivoPerdaApplicationService.Atualizar(motivoPerda, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Motivo da Perda atualizada com sucesso!");
        }


        [HttpDelete]
        [Route("v1/motivo-perda/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _motivoPerdaApplicationService.Deletar(codigo, StringHelper.GetIDD(User.Identity.Name));

            return CreateResponse(HttpStatusCode.OK, "Motivo da Perda deletado com sucesso!");

        }

        [HttpGet]
        [Route("v1/motivo-perda/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> ObterPorCodigo(int codigo)
        {
            var motivoPerda = _motivoPerdaApplicationService.ObterPorCodigo(codigo);

            return CreateResponse(HttpStatusCode.OK, motivoPerda);
        }

        [HttpGet]
        [Route("v1/motivos-perda")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] MotivoPerdaPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<MotivoPerda>(request.Filter);

            var pagedList = _motivoPerdaApplicationService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpGet]
        [Route("v1/motivo-perda")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            IEnumerable<MotivoPerdaResponse> listaMotivoPerda = _motivoPerdaApplicationService.ListarTodos<MotivoPerdaResponse>();

            return CreateResponse(HttpStatusCode.OK, listaMotivoPerda);
        }

        [HttpGet]
        [Route("v1/motivo-perda-ativos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarAtivos()
        {
            IEnumerable<MotivoPerdaResponse> listaMotivoPerda = _motivoPerdaApplicationService.ListarFiltrados<MotivoPerdaResponse>(t => t.Ativo);

            return CreateResponse(HttpStatusCode.OK, listaMotivoPerda);
        }
    }
}