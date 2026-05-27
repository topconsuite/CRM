using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITarefaService : IServiceBase<Tarefa>
    {
        PagedList<Tarefa> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Tarefa, bool>> filter);
    }
}
