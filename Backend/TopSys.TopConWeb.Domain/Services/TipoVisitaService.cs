using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TipoVisitaService : ServiceBase<TipoVisita>, ITipoVisitaService
    {
        private ITipoVisitaRepository _tipoVisitaRepository;

        public TipoVisitaService(ITipoVisitaRepository tipoVisitaRepository) : base(tipoVisitaRepository)
        {
            _tipoVisitaRepository = tipoVisitaRepository;
        }

        public PagedList<TipoVisita> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<TipoVisita, bool>> filter)
        {
            var pagedList = _tipoVisitaRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }
    }
}
