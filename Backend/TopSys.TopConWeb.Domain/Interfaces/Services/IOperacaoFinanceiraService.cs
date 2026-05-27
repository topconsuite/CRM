using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IOperacaoFinanceiraService : IServiceBase<OperacaoFinanceira>
    {
        ICollection<OperacaoFinanceira> Listar();
        OperacaoFinanceira ObterPorId(int id, bool tracking = false);
        bool EstaEmUsoOperacaoFinanceira(int id);
    }
}