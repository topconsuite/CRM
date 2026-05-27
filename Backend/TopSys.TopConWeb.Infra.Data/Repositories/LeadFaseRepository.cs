using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class LeadFaseRepository : RepositoryBase<LeadFase>, ILeadFaseRepository
    {
        public LeadFaseRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
