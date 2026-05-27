using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TributacaoReformaRepository : RepositoryBase<TributacaoReforma>, ITributacaoReformaRepository
    {
        public TributacaoReformaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<TributacaoReforma> ListarTodosProducao(string imposto)
        {
            const int SIMPLESREMESSA = 9;
            const int VENDA = 1;

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT id_imp {nameof(TributacaoReforma.Id)}");
            sqlComando.Append($" FROM ger_imp_tribut_reforma");
            sqlComando.Append($" WHERE cod_trans IN({SIMPLESREMESSA}, {VENDA})");
            sqlComando.Append($" AND ativo=1 AND tp_imp='{imposto}'");
            sqlComando.Append($" GROUP BY id_imp ORDER BY id_imp");

            var result = _context.Database.Connection
                .Query<TributacaoReforma>(sqlComando.ToString())
                .Distinct()
                .ToList();
            return result;
        }
    }
}
