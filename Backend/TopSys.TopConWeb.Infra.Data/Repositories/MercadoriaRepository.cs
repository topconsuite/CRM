using Dapper;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class MercadoriaRepository : RepositoryBase<Mercadoria>, IMercadoriaRepository
    {
        public MercadoriaRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public bool ObtemTracoCriadoPeloTech(string codMercadoria)
        {
            var sql = new StringBuilder();

            sql.Append($"SELECT id_tech");
            sql.Append($" FROM fis_mercadoria");
            sql.Append($" WHERE cod= @codMercadoria");

            return _context.Database.Connection.QueryFirstOrDefault<string>(sql.ToString(), new { codMercadoria }) != "";
        }
    }
}
