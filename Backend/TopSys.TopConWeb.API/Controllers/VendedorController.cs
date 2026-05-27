using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.Vendedor;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Application.DTOS.Request.Filtered;
using System.Linq.Expressions;
using LinqKit;
using TopSys.TopConWeb.Application.DTOS.Request.Vendedor;
using TopSys.TopConWeb.API.Security;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class VendedorController : BaseController
    {
        private readonly IVendedorApplicationService _vendedorApplicationService;

        public VendedorController(IVendedorApplicationService vendedorApplicationService)
        {
            _vendedorApplicationService = vendedorApplicationService;
        }

        [HttpGet]
        [Route("v1/vendedores")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos([FromUri] FilteredRequest request)
        {
            IEnumerable<VendedorResponse> listaVendedores;
            if (request != null)
            {
                var filter = UrlFilterParser.Parse<Vendedor>(request.Filter);
                listaVendedores = _vendedorApplicationService.ListarFiltrados<VendedorResponse>(filter);
            }
            else
            {
                listaVendedores = _vendedorApplicationService.ListarTodos<VendedorResponse>();
            }
            
            return CreateResponse(HttpStatusCode.OK, listaVendedores);
        }

        [HttpGet]
        [Route("v1/vendedores-vinculados")]
        [Authorize]
        public Task<HttpResponseMessage> ListarVinculados()
        {
            var filtro = User.Identity.FiltroVendedoresVinculados<Vendedor>("Codigo")
                .And(t => t.Ativo == "S");

            var listaVendedores = _vendedorApplicationService.ListarFiltrados<VendedorResponse>(filtro);

            return CreateResponse(HttpStatusCode.OK, listaVendedores);
        }

        [HttpGet]
        [Route("v1/vendedores-permitidos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPermitidos()
        {
            var filtro = User.Identity.FiltroVendedoresPermitidos<Vendedor>("Codigo")
                .And(t => t.Ativo == "S");

            var listaVendedores = _vendedorApplicationService.ListarFiltrados<VendedorResponse>(filtro);

            return CreateResponse(HttpStatusCode.OK, listaVendedores);
        }

        [HttpGet]
        [Route("v1/todos-vendedores-permitidos")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodosPermitidos()
        {
            var filtro = User.Identity.FiltroVendedoresPermitidos<Vendedor>("Codigo");

            var listaVendedores = _vendedorApplicationService.ListarFiltrados<VendedorResponse>(filtro);

            return CreateResponse(HttpStatusCode.OK, listaVendedores);
        }


        [HttpGet]
        [Route("integrations/sellers")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorListar()
        {
            var result = _vendedorApplicationService.Listar();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/sellers/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorObterPorID([FromUri] int id)
        {
            var result = _vendedorApplicationService.ObterPorId(id);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/sellers/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorObterPorExternalId([FromUri] string external_id)
        {
            var result = _vendedorApplicationService.ObterPorExternalId(external_id);

            return CreateResponse(result);
        }

        [HttpPost]
        [Route("integrations/sellers")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorAdicionar([FromBody] VendedoresRequest request)
        {
            var result = _vendedorApplicationService.Adicionar(request.Vendedores);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/sellers/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorAtualizarPorId([FromUri] int id, [FromBody] VendedorIntegracaoAtualizarRequest request)
        {
            var result = _vendedorApplicationService.AtualizarId(id, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/sellers/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorAtualizarPorExtenalId([FromUri] string external_id, [FromBody] VendedorIntegracaoAtualizarRequest request)
        {
            var result = _vendedorApplicationService.AtualizarExternalId(external_id, request);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/sellers/{id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorDeletarPorId([FromUri] int id)
        {
            var result = _vendedorApplicationService.DeletarPorId(id);

            return CreateResponse(result);
        }

        [HttpDelete]
        [Route("integrations/sellers/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> VendedorDeletarPorExternalId([FromUri] string external_id)
        {
            var result = _vendedorApplicationService.DeletarPorExternalId(external_id);

            return CreateResponse(result);
        }
    }
}