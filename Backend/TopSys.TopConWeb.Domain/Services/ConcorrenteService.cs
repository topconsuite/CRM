using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ConcorrenteService : ServiceBase<Concorrente>, IConcorrenteService
    {
        private readonly IConcorrenteRepository _concorrenteRepository;

        public ConcorrenteService(IConcorrenteRepository concorrenteRepository) : base(concorrenteRepository)
        {
            _concorrenteRepository = concorrenteRepository;
        }

        public PagedList<Concorrente> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Concorrente, bool>> filter)
        {
            var pagedList = _concorrenteRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }
    }
}
