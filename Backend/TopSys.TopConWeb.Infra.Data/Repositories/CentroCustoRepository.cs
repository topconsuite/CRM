using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CentroCustoRepository : RepositoryBase<CentroCusto>, ICentroCustoRepository
    {
        public CentroCustoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public CentroCusto ObterPorIdCentroCusto(int id, bool tracking = false)
        {
            return _context
                .CentroCusto
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public ICollection<CentroCusto> ListarCentroCusto()
        {
            return _context
                .CentroCusto
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public bool EstaEmUsoCentroCusto(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            int result = 0;

            sqlComando.Append($"SELECT COUNT(cc) FROM fin_car WHERE cc={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(cc) FROM fin_cap WHERE cc={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(cc) FROM fin_mov_banco WHERE cc={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(c_custo) FROM fis_filial WHERE c_custo={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }
    }
}
