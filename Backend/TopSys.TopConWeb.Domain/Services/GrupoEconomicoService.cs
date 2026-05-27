using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class GrupoEconomicoService: ServiceBase<GrupoEconomico>, IGrupoEconomicoService
    {
        private IGrupoEconomicoRepository _grupoEconomicoRepository;

        public GrupoEconomicoService(IGrupoEconomicoRepository grupoEconomicoRepository) : base(grupoEconomicoRepository)
        {
            _grupoEconomicoRepository = grupoEconomicoRepository;
        }

        public PagedList<GrupoEconomico> ListarEmOrdemCrescente(int pagina, int porPagina, IFilter filter)
        {
            var pagedList = _grupoEconomicoRepository.ListarEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }

        public IEnumerable<GrupoEconomico> ListarEmOrdemCrescente()
        {
            return _grupoEconomicoRepository.ListarEmOrdemCrescente();
        }
    }
}
