using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IEmpresaRepository : IRepositoryBase<Empresa>
    {
        ICollection<Empresa> ListarEmpresa();
        Empresa ObterPorIdEmpresa(int id, bool tracking = false);
    }
}
