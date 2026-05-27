using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TributacaoPisCofinsRepository : RepositoryBase<TributacaoPisCofins>, ITributacaoPisCofinsRepository
    {
        public TributacaoPisCofinsRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }
        
        public IEnumerable<TributacaoPisCofins> ListarTributacoesDeSaida()
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append($" trib.cod {nameof(TributacaoPisCofins.Codigo)}");
            sqlComando.Append($", trib.Descr {nameof(TributacaoPisCofins.Descricao)}");
            sqlComando.Append(" FROM ger_tribcontrib AS trib");
            sqlComando.Append(" INNER JOIN ger_tribcontribitem AS tribItem");
            sqlComando.Append(" ON trib.Cod = tribItem.Cod");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" tribItem.CstContrib_Cod > 0");
            sqlComando.Append(" AND tribItem.CstContrib_Cod < 50");
            sqlComando.Append(" ORDER BY CAST(trib.cod AS UNSIGNED)");

            var result = _context.Database.Connection
                .Query<TributacaoPisCofins>(sqlComando.ToString())
                .Distinct()
                .ToList();
            return result;
        }
    }
}