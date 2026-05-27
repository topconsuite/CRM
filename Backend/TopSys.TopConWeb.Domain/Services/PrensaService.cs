using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class PrensaService : ServiceBase<Prensa>, IPrensaService
    {
        private readonly IPrensaRepository _prensaRepository;

        public PrensaService(IPrensaRepository prensaRepository) : base(prensaRepository)
        {
            _prensaRepository = prensaRepository;
        }
    }
}
