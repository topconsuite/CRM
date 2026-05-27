using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ISituacaoFinanceiraRepository : IRepositoryBase<SituacaoFinanceira>
    {
        ICollection<SituacaoFinanceira> ListarSituacaoFinanceira();
        SituacaoFinanceira ObterPorIdSituacaoFinanceira(int id, bool tracking = false);
        bool EstaEmUsoSituacaoFinanceira(int id);
        bool ValidaOperacaoBaixa(int codigoOperacao);
    }
}