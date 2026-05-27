using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class RepasseReajusteRepository : RepositoryBase<RepasseReajuste>, IRepasseReajusteRepository
    {
        private IDatabaseRepository _databaseRepository;

        public RepasseReajusteRepository(AppDataContext context, IDatabaseRepository databaseRepository) : base(context)
        {
            _databaseRepository = databaseRepository;
        }

        public RepasseReajuste ObterVigente(DateTime dataBase)
        {
            return _context.RepasseReajustes
                .Where(t => t.DataInicioValidade <= dataBase)
                .OrderByDescending(t => t.DataInicioValidade)
                .FirstOrDefault();
        }

        public void AdicionarVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_reajuste_item_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_reajuste_item c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"REPLACE INTO topsys.con_reaj_bomba_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_reaj_bomba c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.ano_contrato={anoContrato}");
            sqlComando.Append($" and c.num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void ExcluirVersaoContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_reajuste_item_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_reaj_bomba_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_contrato_reajuste_versao");
            sqlComando.Append($" WHERE num_versao={numVersao}");
            sqlComando.Append($" AND usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void AdicionarContrato(int codUsina, int anoContrato, int numeroContrato, int numVersao)
        {
            StringBuilder sqlComando = new StringBuilder();

            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_reajuste_item_versao", "con_reajuste_item");

            sqlComando.Append($"REPLACE INTO topsys.con_reajuste_item");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_reajuste_item_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_reaj_bomba_versao", "con_reaj_bomba");

            sqlComando.Append($"REPLACE INTO topsys.con_reaj_bomba");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_reaj_bomba_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND ano_contrato={anoContrato}");
            sqlComando.Append($" AND num_contrato={numeroContrato}");
            sqlComando.Append($" AND num_versao={numVersao};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }

        public void ExcluirContrato(int codUsina, int anoContrato, int numeroContrato)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_reajuste_item");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();

            sqlComando.Append($"DELETE FROM topsys.con_reaj_bomba");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and ano_contrato={anoContrato}");
            sqlComando.Append($" and num_contrato={numeroContrato};");
            _context.Database.Connection.Execute(sqlComando.ToString());
            sqlComando.Clear();
        }
    }
}
