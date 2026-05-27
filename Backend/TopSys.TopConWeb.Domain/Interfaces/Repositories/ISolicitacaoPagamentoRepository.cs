using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ISolicitacaoPagamentoRepository
    {
        void Adicionar(SolicitacaoPagamento solicitacaoPagamento);
    }
}
