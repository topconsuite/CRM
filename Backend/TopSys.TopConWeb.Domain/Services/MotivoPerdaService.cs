using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class MotivoPerdaService : ServiceBase<MotivoPerda>, IMotivoPerdaService
    {
        private readonly IMotivoPerdaRepository _motivoPerdaRepository;

        public MotivoPerdaService(IMotivoPerdaRepository motivoPerdaRepository) : base(motivoPerdaRepository)
        {
            _motivoPerdaRepository = motivoPerdaRepository;
        }

        public PagedList<MotivoPerda> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<MotivoPerda, bool>> filter)
        {
            var pagedList = _motivoPerdaRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }
    }
}
