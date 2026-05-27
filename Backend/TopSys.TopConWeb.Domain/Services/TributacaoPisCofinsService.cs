using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TributacaoPisCofinsService : ServiceBase<TributacaoPisCofins>, ITributacaoPisCofinsService
    {
        private ITributacaoPisCofinsRepository _tributacaoPisCofinsRepository;

        public TributacaoPisCofinsService(ITributacaoPisCofinsRepository tributacaoPisCofinsRepository) : base(tributacaoPisCofinsRepository)
        {
            _tributacaoPisCofinsRepository = tributacaoPisCofinsRepository;
        }

        public IEnumerable<TributacaoPisCofins> ListarTributacoesDeSaida()
        {
            return _tributacaoPisCofinsRepository.ListarTributacoesDeSaida();
        }
    }
}