using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Empresa> ListarEmpresa()
        {
            return _context
                .Empresas
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public Empresa ObterPorIdEmpresa(int id, bool tracking = false)
        {
            return _context
                .Empresas
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }
    }
}
