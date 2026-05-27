using Dapper;
using System;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class ParametroFinanceiroRepository : IParametroFinanceiroRepository
    {
        protected AppDataContext _context;

        public ParametroFinanceiroRepository(AppDataContext context)
        {
            _context = context;
        }

        public int ObterOperacaoCartaoPeloCodigoDaEmpresa(int empresa)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT oper_cartao ");
            sql.Append("FROM  topsys.fin_parametro ");
            sql.Append("WHERE ");
            sql.Append($"emp = @EMPRESA");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sql.ToString(), new
            {
                EMPRESA = empresa
            });
        }

        public int ObterOperacaoRecebimentoDoClientePeloCodigoUsina(int codigoUsinaObra)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT oper_cred_cli ");
            sql.Append("FROM  topsys.fin_parametro f");
            sql.Append(" INNER JOIN con_usina c");
            sql.Append(" ON c.emp_filial DIV 1000 = f.emp");
            sql.Append(" WHERE c.cod= @USINACOD");
            sql.Append(" AND inicio_validade<=now()");
            sql.Append(" ORDER BY inicio_validade DESC LIMIT 1");


            return _context.Database.Connection.QueryFirstOrDefault<int>(sql.ToString(), new
            {
                USINACOD = codigoUsinaObra
            });
        }

        public DateTime? ObterMesAbertoPorEmpresa(int empresa)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT MAX(mes_ano_compet) comp");
            sql.Append(" FROM fin_parametro");
            sql.Append(" WHERE emp=@empresa");
            sql.Append(" GROUP BY emp");

            return _context.Database.Connection.QueryFirstOrDefault<DateTime?>(sql.ToString(), new { empresa });
        }

        public ParametroFinanceiroCheque ObterParametroChequePorEmpresa(int empresa)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT");
            sql.Append($" oper_pad_incl_chq {nameof(ParametroFinanceiroCheque.OperacaoPadraoInclusao)},");
            sql.Append($" port_padrao_chq {nameof(ParametroFinanceiroCheque.PortadorPadrao)},");
            sql.Append($" sit_padrao_chq {nameof(ParametroFinanceiroCheque.SituacaoPadrao)}");
            sql.Append($" FROM fin_parametro");
            sql.Append($" WHERE emp=@empresa");
            sql.Append($" AND inicio_validade<=curdate()");
            sql.Append($" ORDER BY inicio_validade DESC");

            return _context.Database.Connection.QueryFirstOrDefault<ParametroFinanceiroCheque>(sql.ToString(), new { empresa });
        }

        public int ObterOperacaoMovimentoBancoAdiantamentoCliente(int codigoUsinaObra)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT oper_mov_bco_ad ");
            sql.Append("FROM topsys.fin_parametro f");
            sql.Append(" INNER JOIN con_usina c");
            sql.Append(" ON c.emp_filial DIV 1000 = f.emp");
            sql.Append(" WHERE c.cod= @USINACOD");
            sql.Append(" AND inicio_validade<=now()");
            sql.Append(" ORDER BY inicio_validade DESC LIMIT 1");

            return _context.Database.Connection.QueryFirstOrDefault<int>(sql.ToString(), new
            {
                USINACOD = codigoUsinaObra
            });
        }
    }
}
