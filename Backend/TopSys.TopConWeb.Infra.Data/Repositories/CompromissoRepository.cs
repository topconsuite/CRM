using Dapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CompromissoRepository : RepositoryBase<Compromisso>, ICompromissoRepository
    {
        public CompromissoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        new public void Adicionar(Compromisso compromisso)
        {
            var sqlComando = compromisso.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            compromisso.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
        }

        public PagedList<Compromisso> ListarEmOrdemDecrescentePorHorario(int pagina, int porPagina, Expression<Func<Compromisso, bool>> filter)
        {
            return _context.Compromisso
                   .OrderBy(t => t.HoraInicio)
                   .Where(filter)
                   .PagedList(pagina, porPagina, filter);
        }
    }
}
