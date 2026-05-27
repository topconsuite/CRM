using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class UsoRepository : RepositoryBase<Uso>, IUsoRepository
    {
        public UsoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public string ObterDescricaoPersonalizada(int idUso)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT p.descr ");
            sql.Append($" FROM con_uso_desc_person P");
            sql.Append($" WHERE p.uso= @idUso");

            return _context.Database.Connection.QueryFirstOrDefault<string>(sql.ToString(), new { idUso });

        }
    }
}
