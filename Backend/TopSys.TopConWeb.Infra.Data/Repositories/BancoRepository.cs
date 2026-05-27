using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class BancoRepository : RepositoryBase<Conta>, IBancoRepository
    {
        public BancoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public Conta ObterPorIdBanco(int cod, int emp, bool tracking = false)
        {
            return _context
                .Contas
                .Where(t => t.Codigo == cod && t.EmpresaCodigo == emp)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public ICollection<Conta> ListarBanco()
        {
            return _context
                .Contas
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public bool EstaEmUsoBanco(int cod, int emp)
        {
            StringBuilder sqlComando = new StringBuilder();

            int result = 0;

            sqlComando.Append($"SELECT COUNT(liq_bco) FROM fin_car WHERE (bco_port={cod} OR liq_bco={cod}) AND emp={emp}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(liq_bco) FROM fin_cap WHERE liq_bco={cod} AND emp={emp}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(bco) FROM fin_mov_banco WHERE bco={cod} AND emp={emp}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(bco) FROM fin_portador WHERE bco={cod} AND emp={emp}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(bco_padrao) FROM fin_operacao WHERE bco_padrao={cod}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }
    }
}
