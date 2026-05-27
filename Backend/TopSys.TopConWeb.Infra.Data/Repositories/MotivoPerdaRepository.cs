using Dapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class MotivoPerdaRepository : RepositoryBase<MotivoPerda>, IMotivoPerdaRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public MotivoPerdaRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(MotivoPerda motivoPerda)
        {
            var sqlComando = motivoPerda.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            motivoPerda.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_motivo_perda", sqlComando.ToString());
        }

        public PagedList<MotivoPerda> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<MotivoPerda, bool>> filter)
        {
            return _context.MotivosPerda
                    .OrderBy(t => t.Codigo)
                    .Where(filter)
                    .PagedList(pagina, porPagina, filter);
        }
    }
}
