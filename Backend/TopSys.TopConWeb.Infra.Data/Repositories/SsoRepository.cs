using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class SsoRepository : RepositoryBase<ParametrosSSO>, ISsoRepository
    {
        public SsoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }
     }
}
