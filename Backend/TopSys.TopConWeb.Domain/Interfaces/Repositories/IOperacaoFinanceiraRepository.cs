using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IOperacaoFinanceiraRepository : IRepositoryBase<OperacaoFinanceira>
    {
        ICollection<OperacaoFinanceira> ListarOperacaoFinanceira();
        OperacaoFinanceira ObterPorIdOperacaoFinanceira(int id, bool tracking = false);
        bool EstaEmUsoOperacaoFinanceira(int id);
    }
}