using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class LeadContatoService : ServiceBase<LeadContato>, ILeadContatoService
    {
        private readonly ILeadContatoRepository _leadContatoRepository;

        public LeadContatoService(ILeadContatoRepository leadContatoRepository) : base(leadContatoRepository)
        {
            _leadContatoRepository = leadContatoRepository;
        }
    }
}
