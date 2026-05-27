using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class LogGeralRepository : RepositoryBase<LogGeral>, ILogGeralRepository
    {
        public LogGeralRepository(AppDataContext context) : base(context)
        {
        }

        new public void Adicionar(LogGeral logGeral)
        {
            var sqlComando = logGeral.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
        }
    }
}
