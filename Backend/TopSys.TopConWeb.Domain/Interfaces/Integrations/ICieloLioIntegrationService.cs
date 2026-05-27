using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.CartaoPagamentoIntegracao;

namespace TopSys.TopConWeb.Domain.Interfaces.Integrations
{
    public interface ICieloLioIntegrationService
    {
        bool EnviarSolicitacaoPagamento(CieloLioDados cieloLioDados, SolicitacaoPagamento solicitacaoPagamento);
    }
}
