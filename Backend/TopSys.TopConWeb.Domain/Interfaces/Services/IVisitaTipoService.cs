using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IVisitaTipoService : IServiceBase<VisitaTipo>
    {
        PagedList<VisitaTipo> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<VisitaTipo, bool>> filter);
    }
}
