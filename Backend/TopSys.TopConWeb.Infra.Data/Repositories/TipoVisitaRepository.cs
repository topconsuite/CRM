using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TipoVisitaRepository : RepositoryBase<TipoVisita>, ITipoVisitaRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public TipoVisitaRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(TipoVisita tipoVisita)
        {
            var sqlComando = tipoVisita.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            tipoVisita.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_tipo_visita", sqlComando.ToString());
        }

        public PagedList<TipoVisita> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<TipoVisita, bool>> filter)
        {
            return  _context.TiposVisitas
                    .OrderBy(t => t.Codigo)
                    .Where(filter)
                    .PagedList(pagina, porPagina, filter);
        }
    }
}
