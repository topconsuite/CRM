using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICentroCustoService : IServiceBase<CentroCusto>
    {
        ICollection<CentroCusto> Listar();
        CentroCusto ObterPorId(int id, bool tracking = false);
        bool EstaEmUsoCentroCusto(int id);
    }
}
