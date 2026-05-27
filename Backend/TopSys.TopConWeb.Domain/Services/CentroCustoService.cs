using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class CentroCustoService : ServiceBase<CentroCusto>, ICentroCustoService
    {
        private readonly ICentroCustoRepository _centroCustoRepository;

        public CentroCustoService(ICentroCustoRepository centroCustoRepository) : base(centroCustoRepository)
        {
            _centroCustoRepository = centroCustoRepository;
        }

        public ICollection<CentroCusto> Listar()
        {
            return _centroCustoRepository.ListarCentroCusto();
        }

        public CentroCusto ObterPorId(int id, bool tracking = false)
        {
            return _centroCustoRepository.ObterPorIdCentroCusto(id, tracking);
        }

        public bool EstaEmUsoCentroCusto(int id)
        {
            return _centroCustoRepository.EstaEmUsoCentroCusto(id);
        }
    }
}
