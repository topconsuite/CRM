using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class DemaisServicosRepository : RepositoryBase<DemaisServicos>, IDemaisServicosRepository
    {
        private IDatabaseRepository _databaseRepository;

        public DemaisServicosRepository(AppDataContext context, IDatabaseRepository databaseRepository) : base(context)
        {
            _context = context;
            _databaseRepository = databaseRepository;
        }

        public PagedList<DemaisServicos> ListaEmOrdemCrescente(int pagina, int porPagina, Expression<Func<DemaisServicos, bool>> filter)
        {
            var pagedList = _context.DemaisServicos
                .OrderBy(t => new { t.Codigo })
                .Include(t => t.Usina)
                .Include(t => t.Mercadoria)
                .Include(t => t.Unidade)
                .Where(filter)
                .PagedList(pagina, porPagina, filter);

            return pagedList;   
        }
        public void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"REPLACE INTO topsys.con_obras_dem_serv_versao");
            sqlComando.Append($" SELECT {numVersao}, c.* from topsys.con_obras_dem_serv c");
            sqlComando.Append($" where c.usina={codUsina}");
            sqlComando.Append($" and c.obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_obras_dem_serv_versao");
            sqlComando.Append($" where num_versao={numVersao}");
            sqlComando.Append($" and usina={codUsina}");
            sqlComando.Append($" and obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public void AdicionarContrato(int codUsina, int numVersao, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            var colunas = _databaseRepository.ObterColunasEmComumEntreTabelas("con_obras_dem_serv_versao", "con_obras_dem_serv");

            sqlComando.Append($"REPLACE INTO topsys.con_obras_dem_serv");
            sqlComando.Append($" SELECT {colunas} FROM topsys.con_obras_dem_serv_versao");
            sqlComando.Append($" WHERE usina={codUsina}");
            sqlComando.Append($" AND obra={numObra}");
            sqlComando.Append($" AND num_versao={numVersao};");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public void ExcluirContrato(int codUsina, int numObra)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"DELETE FROM topsys.con_obras_dem_serv");
            sqlComando.Append($" where usina={codUsina}");
            sqlComando.Append($" and obra={numObra};");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }
    }
}
