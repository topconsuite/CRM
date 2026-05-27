using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITarefaRepository : IRepositoryBase<Tarefa>
    {
        PagedList<Tarefa> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Tarefa, bool>> filter);
    }
}
