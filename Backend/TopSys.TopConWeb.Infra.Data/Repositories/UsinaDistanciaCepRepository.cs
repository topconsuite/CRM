using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class UsinaDistanciaCepRepository : RepositoryBase<UsinaDistanciaCep> ,IUsinaDistanciaCepRepository
    {
        
        public UsinaDistanciaCepRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
