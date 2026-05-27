using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IEmpresaService : IServiceBase<Empresa>
    {
        ICollection<Empresa> Listar();
        Empresa ObterPorId(int id, bool tracking = false);
    }
}
