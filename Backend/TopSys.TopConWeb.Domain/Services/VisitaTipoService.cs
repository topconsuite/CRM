using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class VisitaTipoService : ServiceBase<VisitaTipo>, IVisitaTipoService
    {
        private IVisitaTipoRepository _tipoVisitaRepository;

        public VisitaTipoService(IVisitaTipoRepository tipoVisitaRepository) : base(tipoVisitaRepository)
        {
            _tipoVisitaRepository = tipoVisitaRepository;
        }

        public PagedList<VisitaTipo> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<VisitaTipo, bool>> filter)
        {
            var pagedList = _tipoVisitaRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }
    }
}
