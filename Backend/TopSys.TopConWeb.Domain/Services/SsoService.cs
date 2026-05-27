using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;

namespace TopSys.TopConWeb.Domain.Services
{
    public class SsoService : ServiceBase<ParametrosSSO>, ISsoService
    {
        private readonly ISsoRepository _repository;
        public SsoService(ISsoRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
