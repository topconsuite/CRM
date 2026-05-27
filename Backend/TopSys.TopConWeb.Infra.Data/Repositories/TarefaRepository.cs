using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TarefaRepository : RepositoryBase<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        new public void Adicionar(Tarefa tarefa)
        {
            var sqlComando = tarefa.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            tarefa.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
        }

        public PagedList<Tarefa> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Tarefa, bool>> filter)
        {
            return _context.Tarefa
                   .OrderBy(t => t.Horario)
                   .Where(filter)
                   .PagedList(pagina, porPagina, filter);
        }
    }
}
