using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class SituacaoFinanceiraRepository : RepositoryBase<SituacaoFinanceira>, ISituacaoFinanceiraRepository
    {
        public SituacaoFinanceiraRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public SituacaoFinanceira ObterPorIdSituacaoFinanceira(int id, bool tracking = false)
        {
            return _context
                .SituacaoFinanceira
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public ICollection<SituacaoFinanceira> ListarSituacaoFinanceira()
        {
            return _context
                .SituacaoFinanceira
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public bool EstaEmUsoSituacaoFinanceira(int id)
        {
            StringBuilder sqlComando = new StringBuilder();

            int result = 0;

            sqlComando.Append($"SELECT COUNT(sit) FROM fin_car WHERE sit={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(sit) FROM fin_cap WHERE sit={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(sit) FROM fin_portador WHERE sit={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(situacao) FROM con_tipo_cobranca WHERE situacao={id}");

            result += _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }

        public bool ValidaOperacaoBaixa(int codigoOperacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Clear();
            sqlComando.Append($"SELECT COUNT(cod) FROM fin_operacao");
            sqlComando.Append($" WHERE cod={codigoOperacao}");
            sqlComando.Append($" AND sub_sist='CR'");
            sqlComando.Append($" AND ib='B'");

            int result = _context.Database.Connection.QueryFirstOrDefault<int>(sqlComando.ToString());

            return (result > 0);
        }
    }
}
