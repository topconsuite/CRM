using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IGrupoEconomicoRepository: IRepositoryBase<GrupoEconomico>
    {
       PagedList<GrupoEconomico> ListarEmOrdemCrescente(int pagina, int porPagina, IFilter filter);
       IEnumerable<GrupoEconomico> ListarEmOrdemCrescente();

    }
}
