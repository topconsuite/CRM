using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class LiberacaoAcessoRepository : RepositoryBase<GrupoAcesso>, ILiberacaoAcessoRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public LiberacaoAcessoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        new public void Adicionar(GrupoAcesso grupoAcesso)
        {
            var sqlComando = grupoAcesso.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            grupoAcesso.Codigo = _context.Database.Connection.QueryFirstOrDefault<int>("SELECT LAST_INSERT_ID()");
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "usr_grupo_liberacao_acesso", sqlComando.ToString());
        }
        public PagedList<GrupoAcesso> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<GrupoAcesso, bool>> filter)
        {
            var query = _context.GruposAcessos
                                .Include(g => g.LiberacoesAcessos)
                                .Where(g => g.LiberacoesAcessos.Any(l => l.Grupo == g.Codigo && l.Usuario == null && l.TipoLiberacao == TipoLiberacao.APROVACOES));

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var PagedList = query
                    .OrderBy(t => t.Codigo)
                    .PagedList(pagina, porPagina, filter);

            return PagedList;
        }

        public LiberacaoAcesso ObterLiberacaoAcessoPorUsuario(string usuario)
        {
            return _context.LiberacoesAcessos
                .FirstOrDefault(x => x.Usuario == usuario && x.TipoLiberacao == TipoLiberacao.APROVACOES);
        }
        public IEnumerable<LiberacaoAcesso> ListarUsuariosPorGrupoAcesso(int grupoAcessoCodigo)
        {
            var result = _context.LiberacoesAcessos
                .Where(x => x.Grupo == grupoAcessoCodigo && x.Usuario != null && x.TipoLiberacao == TipoLiberacao.APROVACOES);

            return result;
        }

        public IEnumerable<PeriodoAusenciaUsuario> ListarPeriodosAusenciaPorUsuario(string usuario)
        {
            var afastamento = _context.PeriodosAusenciaUsuarios
                .Where(x => x.Usuario == usuario && x.TipoAusencia == "AFASTAMENTO" && x.TipoLiberacao == TipoLiberacao.APROVACOES)
                .FirstOrDefault();

            var ausencia = _context.PeriodosAusenciaUsuarios
                .Where(x => x.Usuario == usuario && x.TipoAusencia == "AUSENCIA" && x.TipoLiberacao == TipoLiberacao.APROVACOES)
                .FirstOrDefault();

            var ferias = _context.PeriodosAusenciaUsuarios
                .Where(x => x.Usuario == usuario && x.TipoAusencia == "FERIAS" && x.TipoLiberacao == TipoLiberacao.APROVACOES)
                .OrderByDescending(x => x.Codigo)
                .FirstOrDefault();

            var periodosAusencia = new List<PeriodoAusenciaUsuario>();

            if (afastamento != null) periodosAusencia.Add(afastamento);
            if (ausencia != null) periodosAusencia.Add(ausencia);
            if (ferias != null) periodosAusencia.Add(ferias);

            return periodosAusencia;
        }

        public void AtualizarLiberacaoAcessoUsuario(LiberacaoAcesso liberacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"UPDATE usr_liberacao_acesso ");
            sqlComando.Append($"SET hora_entrada=@{nameof(liberacao.HoraEntrada)}, ");
            sqlComando.Append($"hora_saida=@{nameof(liberacao.HoraSaida)}, ");
            sqlComando.Append($"bloquear=@{nameof(liberacao.Bloquear)} ");
            sqlComando.Append($" WHERE codigo=@{nameof(liberacao.Codigo)} ");

            _context.Database.Connection.Execute(sqlComando.ToString(), new { liberacao });
        }
    }
}
