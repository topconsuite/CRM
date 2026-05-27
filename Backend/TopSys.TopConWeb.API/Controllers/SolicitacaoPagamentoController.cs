using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Request.SolicitacaoPagamento;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class SolicitacaoPagamentoController : BaseController
    {
        private readonly ICartaoPagamentoIntegracaoApplicationService _cartaoPagamentoIntegracaoAppService;

        public SolicitacaoPagamentoController(ICartaoPagamentoIntegracaoApplicationService cartaoPagamentoIntegracaoAppService)
        {
            _cartaoPagamentoIntegracaoAppService = cartaoPagamentoIntegracaoAppService;
        }

        [HttpPost]
        [Route("v1/solicitacao-pagamento")]
        [Authorize]
        public Task<HttpResponseMessage> EnviarSolicitacaoPagamento([FromBody] SolicitacaoPagamentoRequest solicitacaoPagamentoRequest)
        {
            solicitacaoPagamentoRequest.Solicitante = User.Identity.Name;
            _cartaoPagamentoIntegracaoAppService.EnviarSolicitacaoPagamento(solicitacaoPagamentoRequest);

            return CreateResponse(HttpStatusCode.OK, "Enviado com sucesso!");
        }
    }
}