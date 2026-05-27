using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using TopSys.TopConWeb.API.Converters;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Inclusao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Constants.CondicaoPagamento;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class CondicaoPagamentoController : BaseController
    {
        private readonly ICondicaoPagamentoApplicationService _condicaoPagamentoAppService;

        public CondicaoPagamentoController(ICondicaoPagamentoApplicationService condicaoPagamentoAppService)
        {
            _condicaoPagamentoAppService = condicaoPagamentoAppService;
        }

        [HttpGet]
        [Route("v1/condicoesPagamento/usina/{idUsina}/data/{data}/intervenienteTipo/{intervenienteTipo}/segmentacao/{segmentacao}")]
        [Authorize]
        public Task<HttpResponseMessage> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao)
        {
            var condicoesPagamento = _condicaoPagamentoAppService.ListarPorUsinaDataIntervenienteTipo(idUsina, data, intervenienteTipo, segmentacao);

            return CreateResponse(HttpStatusCode.OK, condicoesPagamento);
        }

        [HttpGet]
        [Route("v1/condicaoPagamento/{idCondicaoPagamento}/usina/{idUsina}/preco-unitario-tabela/{precoUnitarioTabela}/valor-adicional")]
        [Authorize]
        public Task<HttpResponseMessage> ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela)
        {
            var valorAdicional = _condicaoPagamentoAppService.ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(idCondicaoPagamento, idUsina, precoUnitarioTabela);

            return CreateResponse(HttpStatusCode.OK, valorAdicional);
        }

        [HttpGet]
        [Route("v1/condicoes-pagamento")]
        [Authorize]
        public Task<HttpResponseMessage> Listar([FromUri] CondicoesPagamentoPagedRequest request)
        {
            var urlFilter = UrlFilterParser.Parse<CondicaoPagamento>(request.Filter);

            var pagedList = _condicaoPagamentoAppService.Listar(request.Pagina, request.PorPagina, urlFilter);

            return CreatePagedResponse(HttpStatusCode.OK, pagedList);
        }

        [HttpDelete]
        [Route("v1/condicoes-pagamento/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> Deletar(int codigo)
        {
            _condicaoPagamentoAppService.Deletar(codigo);

            return CreateResponse(HttpStatusCode.OK, "Serviço deletado com Sucesso");
        }

        [HttpPost]
        [Route("v1/condicoes-pagamento")]
        [Authorize]
        public Task<HttpResponseMessage> Adicionar([FromBody] CondicaoPagamentoInclusaoRequest request)
        {
            request.IdCadastro = StringHelper.GetIDD(User.Identity.Name);

            _condicaoPagamentoAppService.Adicionar(request);

            return CreateResponse(HttpStatusCode.OK, "Serviço Adicionado com Sucesso");
        }

        [HttpPatch]
        [Route("v1/condicoes-pagamento")]
        [Authorize]
        public Task<HttpResponseMessage> Atualizar([FromBody] CondicaoPagamentoAlteracaoRequest request)
        {

            request.IdAtualizacao = StringHelper.GetIDD(User.Identity.Name);
            _condicaoPagamentoAppService.Atualizar(request);

            return CreateResponse(HttpStatusCode.OK, "Serviço Atualizado com Sucesso");
        }


        [HttpGet]
        [Route("v1/condicoes-pagamento/possui-obras-utizando/{codigo}")]
        [Authorize]
        public Task<HttpResponseMessage> PossuiObrasUtilizando(int codigo)
        {
            var possuiObraUtilizando = _condicaoPagamentoAppService.PossuiObrasUtilizando(codigo)
;
            return CreateResponse(HttpStatusCode.OK, possuiObraUtilizando);
        }
        
        [HttpPost]
        [Route("integrations/payment-condition")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CondicaoPagamentoAdicionar([FromBody] CondicoesPagamentoRequest request)
        {
            var result = _condicaoPagamentoAppService.AdicionarIntegration(request.CondicoesPagamento);
            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/payment-condition/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CondicaoPagamentoAtualizarPorCodigo(int code, [FromBody] CondicoesPagamentoAtualizarRequest request)
        {

            var result = _condicaoPagamentoAppService.AtualizaPorIdIntegration(code, request);

            return CreateResponse(result);
        }

        [HttpPatch]
        [Route("integrations/payment-condition/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CondicaoPagamentoAtualizarPorIdExterno([FromUri(Name = "external_id")] string externalId, [FromBody] CondicoesPagamentoAtualizarRequest request)
        {
            var result = _condicaoPagamentoAppService.AtualizarPorExternalIdIntegration(externalId, request);

            return CreateResponse(result);
        }
        
        [HttpGet]
        [Route("integrations/payment-condition")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CondicaoPagamentoListar()
        {
            var result = _condicaoPagamentoAppService.ListarIntegration();

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/payment-condition/{code}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CondicaoPagamentoObterPorCodigoIntegration(int code)
        {
            var result = _condicaoPagamentoAppService.ObterPorIdIntegration(code);

            return CreateResponse(result);
        }

        [HttpGet]
        [Route("integrations/payment-condition/by-external-id/{external_id}")]
        [ApiKeyAuthorize]
        public Task<HttpResponseMessage> CondicaoPagamentoObterPorIdExterno([FromUri(Name = "external_id")] string externalId)
        {
            var result = _condicaoPagamentoAppService.ObterPorExternalIdIntegration(externalId);

            return CreateResponse(result);
        }

    }

}