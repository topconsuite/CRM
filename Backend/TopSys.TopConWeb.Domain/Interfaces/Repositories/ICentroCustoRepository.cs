using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICentroCustoRepository : IRepositoryBase<CentroCusto>
    {
        ICollection<CentroCusto> ListarCentroCusto();
        CentroCusto ObterPorIdCentroCusto(int id, bool tracking = false);
        bool EstaEmUsoCentroCusto(int id);
    }
}