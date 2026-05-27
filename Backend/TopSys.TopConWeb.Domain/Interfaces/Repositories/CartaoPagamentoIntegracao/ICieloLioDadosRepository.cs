using TopSys.TopConWeb.Domain.Entities.CartaoPagamentoIntegracao;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories.CartaoPagamentoIntegracao
{
    public interface ICieloLioDadosRepository
    {
        CieloLioDados ObterPorCartaoBandeiraCodigo(int cartaoBandeiraCodigo);
    }
}
