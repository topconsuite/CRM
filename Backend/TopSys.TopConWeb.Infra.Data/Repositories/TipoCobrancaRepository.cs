using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TipoCobrancaRepository : RepositoryBase<TipoCobranca>, ITipoCobrancaRepository
    {
        public TipoCobrancaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<TipoCobranca> ListarPorCondicaoPagamento(int idCondicaoPagamento)
        {
            StringBuilder sqlComando = new StringBuilder();
            
            sqlComando.Append("SELECT");
            sqlComando.Append(" tc.cod tipoCobrancaCodigo");
            sqlComando.Append(" FROM ger_cond_pag cp");
            sqlComando.Append(" CROSS JOIN ger_geral tc");
            sqlComando.Append(" ON tc.cod>=500 AND tc.cod<=599");
            sqlComando.Append(" AND CONCAT(',',cp.tipos_de_cobran,',') LIKE CONCAT('%,',tc.cod,',%')");
            sqlComando.Append(" WHERE cp.cod=@ID_CONDICAO_PAGAMENTO");
            
            var result = _context.Database.Connection
                .Query<int>(sqlComando.ToString(), new { ID_CONDICAO_PAGAMENTO = idCondicaoPagamento })
                .Distinct()
                .ToList();

            var tiposCobranca = _context.TiposCobranca
                .Include(t => t.Portador)
                .Include(t => t.Portador.Conta)
                .Where(t => result.Contains(t.Codigo))
                .AsNoTracking()
                .ToList();

            return tiposCobranca;
            
        }

        public new IEnumerable<TipoCobranca> ListarTodos()
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" tc.cod tipoCobrancaCodigo");
            sqlComando.Append(" FROM ger_cond_pag cp");
            sqlComando.Append(" CROSS JOIN ger_geral tc");
            sqlComando.Append(" ON tc.cod>=500 AND tc.cod<=599");
            sqlComando.Append(" AND CONCAT(',',cp.tipos_de_cobran,',') LIKE CONCAT('%,',tc.cod,',%')");

            var result = _context.Database.Connection
                .Query<int>(sqlComando.ToString())
                .Distinct()
                .ToList();

            var tiposCobranca = _context.TiposCobranca
                .Include(t => t.Portador)
                .Include(t => t.Portador.Conta)
                .Where(t => result.Contains(t.Codigo))
                .AsNoTracking()
                .ToList();

            return tiposCobranca;
        }

        public TipoCobranca GetByCode(int code)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append($" tipo_cobranca {nameof(TipoCobranca.Codigo)}");
            sqlComando.AppendLine(" FROM");
            sqlComando.AppendLine(" con_tipo_cobranca");
            sqlComando.Append($" WHERE tipo_cobranca = @Code");

            var result = _context.Connection.QueryFirstOrDefault<TipoCobranca>(sqlComando.ToString(), new { Code = code });

            return result;
        }
    }
}
