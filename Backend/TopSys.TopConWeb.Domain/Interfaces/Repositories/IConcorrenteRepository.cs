using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IConcorrenteRepository : IRepositoryBase<Concorrente>
    {
        PagedList<Concorrente> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<Concorrente, bool>> filter);
    }
}