using TopSys.TopConWeb.Application.DTOS.Request.SolicitacaoPagamento;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.CartaoPagamentoIntegracao;

namespace TopSys.TopConWeb.Application
{
    public class CartaoPagamentoIntegracaoApplicationService : ICartaoPagamentoIntegracaoApplicationService
    {
        private readonly ICieloLioIntegrationService _cieloLioIntegrationService;
        private readonly ICieloLioDadosRepository _cieloLioDadosRepository;
        private readonly ISolicitacaoPagamentoRepository _solicitacaoPagamentoRepository;

        public CartaoPagamentoIntegracaoApplicationService(
            ICieloLioIntegrationService cieloLioIntegrationService, 
            ICieloLioDadosRepository cieloLioDadosRepository, 
            ISolicitacaoPagamentoRepository solicitacaoPagamentoRepository
        )
        {
            _cieloLioIntegrationService = cieloLioIntegrationService;
            _cieloLioDadosRepository = cieloLioDadosRepository;
            _solicitacaoPagamentoRepository = solicitacaoPagamentoRepository;
        }

        public void EnviarSolicitacaoPagamento(SolicitacaoPagamentoRequest solicitacaoPagamentoRequest)
        {
            switch (solicitacaoPagamentoRequest.CartaoBandeira.TipoIntegracao.ToLower())
            {
                case "cielo_lio":
                    EnviarSolicitacaoPagamentoCieloLio(solicitacaoPagamentoRequest);
                    break;
                default:
                    throw new System.Exception($"Tipo de Integração '{solicitacaoPagamentoRequest.CartaoBandeira.TipoIntegracao.ToLower()}' não implementada nesta versão!");
            }
        }

        private void EnviarSolicitacaoPagamentoCieloLio(SolicitacaoPagamentoRequest solicitacaoPagamentoRequest)
        {
            var solicitacaoPagamento = AutoMapper.Mapper.Map<SolicitacaoPagamento>(solicitacaoPagamentoRequest);

            var cieloLioData = _cieloLioDadosRepository.ObterPorCartaoBandeiraCodigo(solicitacaoPagamento.CartaoBandeiraCodigo);

            var success = _cieloLioIntegrationService.EnviarSolicitacaoPagamento(cieloLioData, solicitacaoPagamento);

            if (!success)
                throw new System.Exception($"Não foi possível enviar solicitação!");

            _solicitacaoPagamentoRepository.Adicionar(solicitacaoPagamento);
        }

    }
}
