using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITipoVisitaRepository : IRepositoryBase<TipoVisita>
    {
        PagedList<TipoVisita> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<TipoVisita, bool>> filter);
    }
}
