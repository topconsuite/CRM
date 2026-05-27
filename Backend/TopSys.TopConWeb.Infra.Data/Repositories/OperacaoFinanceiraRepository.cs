using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Dapper;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class OperacaoFinanceiraRepository : RepositoryBase<OperacaoFinanceira>, IOperacaoFinanceiraRepository
    {
        public OperacaoFinanceiraRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<OperacaoFinanceira> ListarOperacaoFinanceira()
        {
            return _context
                .OperacoesFinanceiras
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public OperacaoFinanceira ObterPorIdOperacaoFinanceira(int id, bool tracking = false)
        {
            return _context
                .OperacoesFinanceiras
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool EstaEmUsoOperacaoFinanceira(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            int result = 0;
            
            sqlComando.Append($"SELECT COUNT(oper) FROM fin_car WHERE oper ={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(op) FROM fin_cap WHERE op={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());
            
            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(op) FROM fin_mov_banco WHERE op={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            
            return result > 0;
            
        }
    }
}