using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TarefaService : ServiceBase<Tarefa>, ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        public TarefaService(ITarefaRepository repository) : base(repository)
        {
            _tarefaRepository = repository;
        }

        public PagedList<Tarefa> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Tarefa, bool>> filter)
        {
            return _tarefaRepository.ListarEmOrdemDecrescentePorHorario(pagina, porPagina, filter);
        }
    }
}
