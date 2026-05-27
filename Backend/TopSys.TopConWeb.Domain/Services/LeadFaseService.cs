using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class LeadFaseService : ServiceBase<LeadFase>, ILeadFaseService
    {
        private readonly ILeadFaseRepository _faseLeadRepository;

        public LeadFaseService(ILeadFaseRepository faseLeadRepository) : base(faseLeadRepository)
        {
            _faseLeadRepository = faseLeadRepository;
        }
    }
}
