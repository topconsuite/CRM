using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TopSys.TopConWeb.Application.DTOS.Response.TransacaoCartao;
using TopSys.TopConWeb.Application.Interfaces;

namespace TopSys.TopConWeb.API.Controllers
{
    [RoutePrefix("api")]
    public class FinanceiroController: BaseController
    {
        private readonly IContasAReceberApplicationService _contasAReceberApplicationService;
        private readonly IMovimentoBancoApplicationService _movimentoBancoApplicationService;

        public FinanceiroController(IContasAReceberApplicationService contasAReceberApplicationService, IMovimentoBancoApplicationService movimentoBancoApplicationService)
        {
            _contasAReceberApplicationService = contasAReceberApplicationService;
            _movimentoBancoApplicationService = movimentoBancoApplicationService;
        }

        [HttpPost]
        [Route("v1/financeiro/contasAReceber/transacaoCartao")]
        [Authorize]
        public Task<HttpResponseMessage> GeraContasAReceberDaOperadora([FromBody] TransacaoCartaoDTO transacao)
        {
            _contasAReceberApplicationService.GeraContasAReceberDaOperadora(transacao.TransacaoId, User.Identity.Name);
            return CreateResponse(HttpStatusCode.OK, "Processo realizado com sucesso!");
        }

        [HttpPost]
        [Route("v1/financeiro/AprovaPagamentoAntecipadoCartaoDeCredito/usina/{usina}/obra/{obra}")]
        [Authorize]
        public Task<HttpResponseMessage> AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int obra)
        {
            _contasAReceberApplicationService.AprovaPagamentoAntecipadoCartaoDeCredito(usina,obra);
            return CreateResponse(HttpStatusCode.OK, "Processo realizado com sucesso!");
        }

        
        [HttpPost]
        [Route("v1/financeiro/AprovaPagamentoAntecipadoCartaoDeCredito/usina/{usina}/obra/{obra}/sequencia-pagamento/{sequenciaPagamento}")]
        [Authorize]
        public Task<HttpResponseMessage> AprovaPagamentoAntecipadoCartaoDeCredito(int usina, int obra, int sequenciaPagamento)
        {
            _contasAReceberApplicationService.AprovaPagamentoAntecipadoCartaoDeCredito(usina, obra, sequenciaPagamento, User.Identity.Name);
            return CreateResponse(HttpStatusCode.OK, "Processo realizado com sucesso!");
        }

        [HttpPost]
        [Route("v1/financeiro/GerarContasAReceberOperadoraEAprovarPagamentoAtencipadoCartaoDeCredito/usina/{usina}/obra/{obra}/sequencia-pagamento/{sequenciaPagamento}")]
        [Authorize]
        public Task<HttpResponseMessage> GerarContasAReceberOperadoraEAprovarPagamentoAtencipadoCartaoDeCredito([FromBody] TransacaoCartaoDTO transacao, int usina, int obra, int sequenciaPagamento)
        {
            _contasAReceberApplicationService.GerarContasAReceberOperadoraEAprovarPagamentoAtencipadoCartaoDeCredito(transacao.TransacaoId, usina, obra, sequenciaPagamento, User.Identity.Name);
            return CreateResponse(HttpStatusCode.OK, "Processo realizado com sucesso!");
        }      
  
        [HttpPost]
        [Route("v1/financeiro/desaprovar-condicao-pagamento/usina/{usina}/contrato-ano/{contratoAno}/contrato-numero/{contratoNumero}/sequencia-pagamento/{sequenciaPagamento}/verifica-movimento-bancario/{verificaMovimentoBancarioConciliado}")]
        [Authorize]
        public Task<HttpResponseMessage> DesaprovarCondicaoPagamento(int usina, int contratoAno, int contratoNumero, int sequenciaPagamento, bool verificaMovimentoBancarioConciliado)
        {
            _contasAReceberApplicationService.DesaprovarCondicaoPagamento(usina, contratoAno, contratoNumero, sequenciaPagamento, User.Identity.Name, verificaMovimentoBancarioConciliado);
            return CreateResponse(HttpStatusCode.OK, "Processo realizado com sucesso!");
        }

        [HttpGet]
        [Route("v1/financeiro/movimento-banco/empresa/{empresaCodigo}/conta/{contaCodigo}/nao-vinculados-car")]
        [Authorize]
        public Task<HttpResponseMessage> ListarNaoVinculadosComContasAReceber(int empresaCodigo, int contaCodigo, [FromUri] DateTime? dataOperacao = null)
        {
            var result = _movimentoBancoApplicationService.ListarNaoVinculadosComContasAReceber(empresaCodigo, contaCodigo, dataOperacao);
            return CreateResponse(HttpStatusCode.OK, result);
        }
    }
}