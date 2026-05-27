using Dapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class OportunidadeTipoRepository : RepositoryBase<OportunidadeTipo>, IOportunidadeTipoRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public OportunidadeTipoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(OportunidadeTipo oportunidadeTipo)
        {
            var sqlComando = oportunidadeTipo.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            oportunidadeTipo.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "con_tipo_oportunidade", sqlComando.ToString());
        }

        public PagedList<OportunidadeTipo> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<OportunidadeTipo, bool>> filter)
        {
            return _context.OportunidadesTipos
                    .OrderBy(t => t.Codigo)
                    .Where(filter)
                    .PagedList(pagina, porPagina, filter);
        }
    }
}
