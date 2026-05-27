using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.Application.DTOS.Request.Filtered;
using TopSys.TopConWeb.Application.DTOS.Response.TributacaoPisCofins;
using TopSys.TopConWeb.Application.DTOS.Response.Vendedor;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class TributacaoPisCofinsController : BaseController
    {
        private readonly ITributacaoPisCofinsApplicationService _tributacaoPisCofinsApplicationService;
        
        public TributacaoPisCofinsController(ITributacaoPisCofinsApplicationService tributacaoPisCofinsApplicationApplicationApplicationService)
        {
            _tributacaoPisCofinsApplicationService = tributacaoPisCofinsApplicationApplicationApplicationService;
        }
        
        [HttpGet]
        [Route("v1/tributacaoPisCofins")]
        [Authorize]
        public Task<HttpResponseMessage> ListarTodos()
        {
            var listaTributacoesPisCofins = _tributacaoPisCofinsApplicationService.ListarTodosDeSaida();
            
            return CreateResponse(HttpStatusCode.OK, listaTributacoesPisCofins);
        }
    }
}