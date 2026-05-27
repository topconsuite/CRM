using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICompromissoRepository : IRepositoryBase<Compromisso>
    {
        PagedList<Compromisso> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Compromisso, bool>> filter);
    }
}
