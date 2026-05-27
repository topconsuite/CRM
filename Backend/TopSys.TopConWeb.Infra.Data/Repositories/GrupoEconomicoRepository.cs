using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class GrupoEconomicoRepository : RepositoryBase<GrupoEconomico>, IGrupoEconomicoRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public GrupoEconomicoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public GrupoEconomico ObterPorId(params object[] keyvalues)
        {
            var codigo = (int)keyvalues.First();

            return _context.GruposEconomicos
            .Include(c => c.Clientes)
            .FirstOrDefault(x => x.Codigo == codigo);
        }

        new public void Adicionar(GrupoEconomico grupoEconomico)
        {
            var sqlComando = grupoEconomico.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            grupoEconomico.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "ger_grupo_economico", sqlComando.ToString());
        }

        new public void Atualizar(GrupoEconomico grupoEconomico)
        {
            var sqlCommand = new StringBuilder();
            sqlCommand.AppendLine("UPDATE topsys.ger_grupo_economico");
            sqlCommand.Append($" SET descricao=@{nameof(GrupoEconomico.Descricao)}");
            sqlCommand.Append($", id_atual=@{nameof(GrupoEconomico.IdAtualizacao)}");
            sqlCommand.Append($" WHERE codigo=@{nameof(GrupoEconomico.Codigo)}");

            _context.Database.Connection.Execute(sqlCommand.ToString(), grupoEconomico);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "ger_grupo_economico", sqlCommand.ToString(), grupoEconomico);
        }

        public PagedList<GrupoEconomico> ListarEmOrdemCrescente(int pagina, int porPagina, IFilter filter)
        {
            var f = (GrupoEconomicoFilter)filter;

            var pagedList = _context.GruposEconomicos
                .OrderBy(t => t.Codigo)
                .Include(c => c.Clientes)
                .Include(g => g.BloqueioMotivo);
            if (f.Codigo != null)
                pagedList = pagedList.Where(x => x.Codigo == f.Codigo);
            if (f.Descricao != null && f.Descricao != "")
                pagedList = pagedList.Where(x => x.Descricao.Contains(f.Descricao));
            if (f.IntervenienteCodigo != null)
                pagedList = pagedList.Where(x => x.Clientes.Where(y => y.Codigo == f.IntervenienteCodigo).Count() > 0);

            return pagedList.PagedList(pagina, porPagina, null);
        }

        public IEnumerable<GrupoEconomico> ListarEmOrdemCrescente()
        {
            var list = _context.GruposEconomicos
                .OrderBy(t => t.Descricao)
                .Include(c => c.Clientes);

            return list;
        }

    }
}
