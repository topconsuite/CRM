using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;


namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly AppDataContext _context;

        public DatabaseRepository(AppDataContext context)
        {
            _context = context;
        }

        public string ObterColunasEmComumEntreTabelas(string origem, string destino)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT GROUP_CONCAT(origem.column_name) AS ColumnNames FROM information_schema.columns origem");
            sqlComando.Append($" INNER JOIN information_schema.columns destino");
            sqlComando.Append($" ON destino.table_schema = 'topsys'");
            sqlComando.Append($" AND destino.table_name = @destino");
            sqlComando.Append($" AND destino.column_name = origem.column_name");
            sqlComando.Append($" WHERE origem.table_schema = 'topsys' AND origem.table_name = @origem;");

            return _context.Database.Connection.Query<string>(sqlComando.ToString(), new {destino = destino, origem = origem }).FirstOrDefault();
        }

        public IEnumerable<string> ObterColunasTabela(string tabela)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"SELECT column_name AS ColumnNames FROM information_schema.columns");
            sqlComando.Append($" WHERE table_schema = 'topsys' AND table_name = @tabela AND column_name <> 'num_versao' AND column_name <> 'dt_versao_criada';");

            return _context.Database.Connection.Query<string>(sqlComando.ToString(), new {tabela = tabela});
        }
    }
}
