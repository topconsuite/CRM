using TopSys.TopConWeb.Application.DTOS.Request.SolicitacaoPagamento;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICartaoPagamentoIntegracaoApplicationService
    {
        void EnviarSolicitacaoPagamento(SolicitacaoPagamentoRequest solicitacaoPagamentoRequest);
    }
}
